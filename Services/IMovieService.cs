using BookMediaDiscoverer.Models;

namespace BookMediaDiscoverer.Services
{
    public interface IMovieService
    {
        Task<List<Movie>> SearchMoviesAsync(string query);
        Task<Movie> GetMovieDetailsAsync(string imdbId);
    }
}