using Library.Models;
using Microsoft.EntityFrameworkCore;

public class BookService
{
    private readonly LibraryContext _context;

    public BookService(LibraryContext context)
    {
        _context = context;
    }

    public async Task<List<Book>> SearchBooks(string query)
    {
        // Retrieve the list of books asynchronously from the database
        var books = await _context.Books.ToListAsync();

        // Perform similarity search with Levenshtein distance
        var similarBooks = books
            .Select(book => new
            {
                book.Id,
                book.Title,
                Distance = LevDistance(query.ToLower(), book.Title.ToLower())
            })
            .Where(x => x.Distance <= 3)
            .OrderBy(x => x.Distance)
            .ToList();

        var bookIds = similarBooks.Select(x => x.Id).ToList();
        var resultBooks = _context.Books.Where(book => bookIds.Contains(book.Id)).ToList();

        return resultBooks;
    }


    internal static int LevDistance(string a, string b)
    {

        if (string.IsNullOrEmpty(a) && string.IsNullOrEmpty(b)) return 0;
        if (string.IsNullOrEmpty(a)) return b.Length;
        if (string.IsNullOrEmpty(b)) return a.Length;

        int lenghtA = a.Length;
        int lenghtB = b.Length;

        var distance = new int[lenghtA + 1, lenghtB + 1];

        for (int i = 0; i <= lenghtA; distance[i, 0] = i++) ;
        for (int j = 0; j <= lenghtB; distance[0, j] = j++) ;

        for (int i = 1; i <= lenghtA; i++)
        {
            for (int j = 1; j <= lenghtB; j++)
            {

                int c = b[j - 1] == a[i - 1] ? 0 : 1;

                distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + c);
            }
        }

        return distance[lenghtA, lenghtB];
    }
}