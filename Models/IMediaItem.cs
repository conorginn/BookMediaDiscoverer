namespace BookMediaDiscoverer.Models
{
    public interface IMediaItem
    {
        string Title { get; set; }
        string Description { get; set; }
        string ImageUrl { get; set; }
        string Year { get; set; }
    }

    public enum MediaType
    {
        Book,
        Movie
    }
}