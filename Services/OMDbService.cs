using System.Text.Json;
using BookMediaDiscoverer.Models;

namespace BookMediaDiscoverer.Services
{
    public class OMDbService : IMovieService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://www.omdbapi.com/";
        private readonly string _apiKey;

        public OMDbService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            // Free API key from http://www.omdbapi.com/apikey.aspx
            _apiKey = "752862ab";
        }

        public async Task<List<Movie>> SearchMoviesAsync(string query)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}?apikey={_apiKey}&s={Uri.EscapeDataString(query)}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"API request failed: {response.StatusCode}");
                    return new List<Movie>();
                }

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<OMDbSearchResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result?.Search == null || result.Response == "False")
                {
                    Console.WriteLine($"No results or API error: {result?.Error}");
                    return new List<Movie>();
                }

                // Get details for each movie (limited to avoid too many requests)
                var movies = new List<Movie>();
                foreach (var movieItem in result.Search.Take(3))
                {
                    if (!string.IsNullOrEmpty(movieItem.ImdbID))
                    {
                        var movie = await GetMovieDetailsAsync(movieItem.ImdbID);
                        if (movie != null)
                            movies.Add(movie);

                        // Small delay to be respectful to the API
                        await Task.Delay(100);
                    }
                }

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
                var response = await _httpClient.GetAsync($"{BaseUrl}?apikey={_apiKey}&i={imdbId}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"API request failed: {response.StatusCode}");
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

    // JSON response classes for OMDb
    public class OMDbSearchResponse
    {
        public List<OMDbMovieItem> Search { get; set; } = new();
        public string TotalResults { get; set; } = string.Empty;
        public string Response { get; set; } = string.Empty;
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