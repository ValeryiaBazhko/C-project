using Library.Models;
using Microsoft.EntityFrameworkCore;

public class BookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<List<Book>> GetAllBooks(int? pageNum = null, int? pageSize = null)
    {
        return await _bookRepository.GetAllBooks(pageNum, pageSize);
    }

    public async Task<int> GetTotNumOfBooks()
    {
        return await _bookRepository.GetTotNumOfBooks();
    }

    public async Task<Book?> GetBookById(int id)
    {

        return await _bookRepository.GetBookById(id);

    }


    public async Task<Book> AddBook(Book book)
    {
        return await _bookRepository.AddBook(book);
    }
    public async Task<bool> UpdateBook(Book book)
    {
        return await _bookRepository.UpdateBook(book);
    }

    public async Task<bool> DeleteBook(int id)
    {
        return await _bookRepository.DeleteBook(id);
    }

    public async Task<List<Book>> SearchBooks(string query)
    {

        var books = await _bookRepository.GetAllBooks();


        var similarBooks = books
            .Select(book => new
            {
                Book = book,
                Distance = LevDistance(query.ToLower(), book.Title.ToLower())
            })
            .Where(x => x.Distance <= 3)
            .OrderBy(x => x.Distance)
            .Select(x => x.Book)
            .ToList();

        return similarBooks;
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