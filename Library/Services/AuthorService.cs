using Library.Models;
using Microsoft.EntityFrameworkCore;

public class AuthorService
{
    private readonly LibraryContext _context;

    public AuthorService(LibraryContext context)
    {
        _context = context;
    }

    public async Task<List<Author>> GetAllAuthors()
    {
        return await _context.Authors.ToListAsync();
    }

    public async Task<Author?> GetAuthorById(int id)
    {
        return await _context.Authors.FindAsync(id);
    }

    public async Task<Author> AddAuthor(Author author)
    {
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
        return author;
    }
}