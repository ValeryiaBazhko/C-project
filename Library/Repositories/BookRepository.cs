using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Models;
using Microsoft.EntityFrameworkCore;

public class BookRepository : IBookRepository
{
    private readonly LibraryContext _context;

    public BookRepository(LibraryContext context)
    {
        _context = context;
    }

    public async Task<List<Book>> GetAllBooks(int? pageNum = null, int? pageSize = null)
    {
        IQueryable<Book> books = _context.Books.OrderBy(b => b.Title);

        if (pageNum.HasValue && pageSize.HasValue && pageNum > 0 && pageSize > 0)
        {
            books = books.Skip((pageNum.Value - 1) * pageSize.Value).Take(pageSize.Value);
        }

        return await books.ToListAsync() ?? new List<Book>();
    }

    public async Task<Book> AddBook(Book book)
    {
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<bool> DeleteBook(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return false;

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Book?> GetBookById(int id)
    {
        return await _context.Books.FindAsync(id);
    }

    public async Task<int> GetTotNumOfBooks()
    {
        return await _context.Books.CountAsync();
    }

    public async Task<List<Book>> SearchBooks(string query)
    {
        return await _context.Books
            .Where(book => book.Title.Contains(query))
            .ToListAsync();
    }

    public async Task<bool> UpdateBook(Book book)
    {
        var exisBook = await _context.Books.FindAsync(book.Id);
        if (exisBook == null) return false;

        exisBook.Title = book.Title;
        exisBook.AuthorId = book.AuthorId;
        exisBook.PublicationYear = book.PublicationYear;

        await _context.SaveChangesAsync();
        return true;
    }
}