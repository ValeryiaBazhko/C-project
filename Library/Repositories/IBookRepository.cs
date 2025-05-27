using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Models;
public interface IBookRepository
{
    Task<List<Book>> GetAllBooks(int? pageNum = null, int? pageSize = null);
    Task<int> GetTotNumOfBooks();
    Task<Book?> GetBookById(int id);
    Task<Book> AddBook(Book book);
    Task<bool> UpdateBook(Book book);
    Task<bool> DeleteBook(int id);
    Task<List<Book>> SearchBooks(string query);
}