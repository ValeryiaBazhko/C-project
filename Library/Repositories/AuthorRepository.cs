using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

public class AuthorRepository : IAuthorRepository
{
    private readonly LibraryContext _context;
    private readonly IMemoryCache _cache;
    private readonly TimeSpan cacheDuration = TimeSpan.FromMinutes(10);

    public AuthorRepository(LibraryContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<List<Author>> GetAllAuthors()
    {
        const string cacheKey = "authors_list";

        if (!_cache.TryGetValue(cacheKey, out List<Author>? authors))
        {
            authors = await _context.Authors.OrderBy(a => a.Name).ToListAsync();

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = cacheDuration
            };

            _cache.Set(cacheKey, authors, cacheOptions);
        }

        return authors!;
    }

    public async Task<Author?> GetAuthorById(int id)
    {
        return await _context.Authors.FindAsync(id);
    }

    public async Task<Author> AddAuthor(Author author)
    {
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();

        _cache.Remove("authors_list");
        return author;
    }
}