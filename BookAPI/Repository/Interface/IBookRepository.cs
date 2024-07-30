using BookAPI.Models;

namespace BookAPI.Repository.Interface
{
    public interface IBookRepository
    {
        void AddBook(Book book);
        IEnumerable<Book> GetBooks();
        void UpdateBook(Book book);
        void SoftDeleteBook(int bookId);        
    }
}
