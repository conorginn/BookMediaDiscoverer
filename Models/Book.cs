namespace BookMediaDiscoverer.Models
{
    public class Book
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<string> Authors { get; set; } = new();
        public string Description { get; set; } = string.Empty;
        public string PublishedDate { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public int? PageCount { get; set; }
        public string ThumbnailUrl { get; set; } = string.Empty;
        public string Isbn { get; set; } = string.Empty;
        public List<string> Categories { get; set; } = new();
        public double AverageRating { get; set; }
        public int RatingsCount { get; set; }
    }
}