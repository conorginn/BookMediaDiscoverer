using BookMediaDiscoverer.Models;

namespace BookMediaDiscoverer.Services
{
    public interface IBookService
    {
        Task<List<Book>> SearchBooksAsync(string query);
        Task<Book> GetBookDetailsAsync(string bookId);
    }
}