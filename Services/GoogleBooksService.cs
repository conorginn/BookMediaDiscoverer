using System.Text.Json;
using BookMediaDiscoverer.Models;

namespace BookMediaDiscoverer.Services
{
    public class GoogleBooksService : IBookService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://www.googleapis.com/books/v1/volumes";

        public GoogleBooksService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Book>> SearchBooksAsync(string query)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}?q={Uri.EscapeDataString(query)}&maxResults=20");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<GoogleBooksResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result?.Items?.Select(item => new Book
                {
                    Id = item.Id,
                    Title = item.VolumeInfo.Title,
                    Authors = item.VolumeInfo.Authors ?? new List<string>(),
                    Description = item.VolumeInfo.Description ?? "No description available",
                    PublishedDate = item.VolumeInfo.PublishedDate,
                    Publisher = item.VolumeInfo.Publisher ?? "Unknown",
                    PageCount = item.VolumeInfo.PageCount,
                    ThumbnailUrl = item.VolumeInfo.ImageLinks?.Thumbnail ?? string.Empty,
                    Isbn = GetIsbn(item.VolumeInfo.IndustryIdentifiers),
                    Categories = item.VolumeInfo.Categories ?? new List<string>(),
                    AverageRating = item.VolumeInfo.AverageRating ?? 0,
                    RatingsCount = item.VolumeInfo.RatingsCount ?? 0
                }).ToList() ?? new List<Book>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching books: {ex.Message}");
                return new List<Book>();
            }
        }

        public async Task<Book> GetBookDetailsAsync(string bookId)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{bookId}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var item = JsonSerializer.Deserialize<GoogleBookItem>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return new Book
            {
                Id = item.Id,
                Title = item.VolumeInfo.Title,
                Authors = item.VolumeInfo.Authors ?? new List<string>(),
                Description = item.VolumeInfo.Description ?? "No description available",
                PublishedDate = item.VolumeInfo.PublishedDate,
                Publisher = item.VolumeInfo.Publisher ?? "Unknown",
                PageCount = item.VolumeInfo.PageCount,
                ThumbnailUrl = item.VolumeInfo.ImageLinks?.Thumbnail ?? string.Empty,
                Isbn = GetIsbn(item.VolumeInfo.IndustryIdentifiers),
                Categories = item.VolumeInfo.Categories ?? new List<string>(),
                AverageRating = item.VolumeInfo.AverageRating ?? 0,
                RatingsCount = item.VolumeInfo.RatingsCount ?? 0
            };
        }

        private static string GetIsbn(List<IndustryIdentifier> identifiers)
        {
            return identifiers?.FirstOrDefault(i => i.Type == "ISBN_13" || i.Type == "ISBN_10")?.Identifier ?? string.Empty;
        }
    }

    // JSON response classes
    public class GoogleBooksResponse
    {
        public List<GoogleBookItem> Items { get; set; } = new();
    }

    public class GoogleBookItem
    {
        public string Id { get; set; } = string.Empty;
        public VolumeInfo VolumeInfo { get; set; } = new();
    }

    public class VolumeInfo
    {
        public string Title { get; set; } = string.Empty;
        public List<string> Authors { get; set; } = new();
        public string Publisher { get; set; } = string.Empty;
        public string PublishedDate { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<IndustryIdentifier> IndustryIdentifiers { get; set; } = new();
        public int? PageCount { get; set; }
        public List<string> Categories { get; set; } = new();
        public double? AverageRating { get; set; }
        public int? RatingsCount { get; set; }
        public ImageLinks ImageLinks { get; set; } = new();
    }

    public class IndustryIdentifier
    {
        public string Type { get; set; } = string.Empty;
        public string Identifier { get; set; } = string.Empty;
    }

    public class ImageLinks
    {
        public string Thumbnail { get; set; } = string.Empty;
    }
}