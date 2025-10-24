using System.Text.Json;
using BookMediaDiscoverer.Models;

namespace BookMediaDiscoverer.Services
{
    public class OMDbService : IMovieService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://www.omdbapi.com/"; // Using HTTPS
        private readonly string _apiKey;

        public OMDbService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiKey = "752862ab"; // Using demo key for testing
        }

        public async Task<List<Movie>> SearchMoviesAsync(string query)
        {
            try
            {
                Console.WriteLine($"Searching for movies: {query}");
                var url = $"{BaseUrl}?apikey={_apiKey}&s={Uri.EscapeDataString(query)}";
                Console.WriteLine($"API URL: {url}");

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"API request failed: {response.StatusCode}");
                    return new List<Movie>();
                }

                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Raw JSON response: {json}");

                var result = JsonSerializer.Deserialize<OMDbSearchResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Check if the API returned an error
                if (result?.Response == "False")
                {
                    Console.WriteLine($"API Error: {result.Error}");
                    return new List<Movie>();
                }

                if (result?.Search == null || !result.Search.Any())
                {
                    Console.WriteLine("No search results found");
                    return new List<Movie>();
                }

                Console.WriteLine($"Found {result.Search.Count} movies in search results");

                // Convert search results to Movie objects with basic info
                var movies = result.Search.Select(item => new Movie
                {
                    ImdbID = item.ImdbID,
                    Title = item.Title,
                    Year = item.Year,
                    Type = item.Type,
                    Poster = item.Poster,
                    // Add placeholder data for other fields
                    Rated = "N/A",
                    Released = "N/A",
                    Runtime = "N/A",
                    Genre = "N/A",
                    Director = "N/A",
                    Writer = "N/A",
                    Actors = "N/A",
                    Plot = "Plot details available in full movie view",
                    Language = "N/A",
                    Country = "N/A",
                    ImdbRating = "N/A"
                }).ToList();

                Console.WriteLine($"Returning {movies.Count} movies");
                return movies;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching movies: {ex.Message}");
                return new List<Movie>();
            }
        }

        public async Task<Movie> GetMovieDetailsAsync(string imdbId)
        {
            try
            {
                Console.WriteLine($"Getting details for movie: {imdbId}");
                var url = $"{BaseUrl}?apikey={_apiKey}&i={imdbId}";

                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Details request failed: {response.StatusCode}");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                var movie = JsonSerializer.Deserialize<Movie>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return movie;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting movie details: {ex.Message}");
                return null;
            }
        }
    }

    // JSON response classes for OMDb SEARCH (not individual movie)
    public class OMDbSearchResponse
    {
        public List<OMDbMovieItem> Search { get; set; } = new();
        public string TotalResults { get; set; } = string.Empty;
        public string Response { get; set; } = string.Empty; // This is for the search response
        public string Error { get; set; } = string.Empty;
    }

    public class OMDbMovieItem
    {
        public string Title { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string ImdbID { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Poster { get; set; } = string.Empty;
    }
}