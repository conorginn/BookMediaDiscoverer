using System.Text.Json;
using BookMediaDiscoverer.Models;

namespace BookMediaDiscoverer.Services
{
    public class OMDbService : IMovieService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://www.omdbapi.com/";
        private readonly string _apiKey;

        public OMDbService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiKey = "752862ab"; 
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

                // Get detailed information for each movie (limited to avoid too many requests)
                var movies = new List<Movie>();
                foreach (var movieItem in result.Search.Take(5)) // Limit to 5 movies
                {
                    if (!string.IsNullOrEmpty(movieItem.ImdbID))
                    {
                        try
                        {
                            var movie = await GetMovieDetailsAsync(movieItem.ImdbID);
                            if (movie != null)
                            {
                                movies.Add(movie);
                                Console.WriteLine($"Retrieved details for: {movie.Title}");
                            }

                            // Small delay to be respectful to the API
                            await Task.Delay(200);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error getting details for {movieItem.Title}: {ex.Message}");
                            // Add basic movie info if detailed fetch fails
                            movies.Add(new Movie
                            {
                                ImdbID = movieItem.ImdbID,
                                Title = movieItem.Title,
                                Year = movieItem.Year,
                                Type = movieItem.Type,
                                Poster = movieItem.Poster,
                                Plot = "Detailed information not available",
                                Rated = "N/A",
                                Runtime = "N/A",
                                Genre = "N/A",
                                Director = "N/A",
                                ImdbRating = "N/A"
                            });
                        }
                    }
                }

                Console.WriteLine($"Successfully retrieved {movies.Count} detailed movie records");
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
                var url = $"{BaseUrl}?apikey={_apiKey}&i={imdbId}&plot=short";

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

                if (movie != null)
                {
                    Console.WriteLine($"Successfully retrieved: {movie.Title}");
                }

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