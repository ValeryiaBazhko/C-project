using System.ComponentModel.DataAnnotations;
using Library.Models;
using Microsoft.EntityFrameworkCore;

public class BookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<List<Book>> GetAllBooks(int? pageNum = null, int? pageSize = null) //  
    {
        return await _bookRepository.GetAllBooks(pageNum, pageSize);
    }

    public async Task<int> GetTotNumOfBooks()
    {
        return await _bookRepository.GetTotNumOfBooks();
    }

    public async Task<Book?> GetBookById(int id)
    {
        Console.WriteLine($"Inside GetBookById with ID: {id}");

        if (id < 0)
        {
            Console.WriteLine($"Throwing ValidationException: Invalid ID {id}");
            throw new ValidationException("Invalid ID");
        }
        
        var book = await _bookRepository.GetBookById(id); 
        return book;
    }



    public async Task<Book> AddBook(Book book) ////
    {
        if (string.IsNullOrWhiteSpace(book.Title))
        {
            throw new ValidationException("Title cannot be empty");
        }

        if (book.Id < 0)
        {
            throw new ValidationException("Invalid ID");
        }

        if (book.PublicationYear <= 0 || book.PublicationYear > DateTime.Now.Year)
        {
            throw new ValidationException("Invalid publication year");
        }

        await _bookRepository.AddBook(book);
        return book;
    }

    public async Task<bool> UpdateBook(Book book) ///
    {
        if(string.IsNullOrWhiteSpace(book.Title)){
            throw new ValidationException("Title cannot be empty");
        }
        if(book.PublicationYear <= 0 || book.PublicationYear > DateTime.Now.Year){
            throw new ValidationException("Invalid publication year");
        }
        return await _bookRepository.UpdateBook(book);
    }

    public async Task<bool> DeleteBook(int id) ////
    {
        return await _bookRepository.DeleteBook(id);
    }

    public async Task<List<Book>> SearchBooks(string query) ////
    {

        var books = await _bookRepository.GetAllBooks();


        var similarBooks = books
            .Select(book => new
            {
                Book = book,
                Distance = FinalDistance(query.ToLower(), book.Title.ToLower())
            })
            .Where(x => x.Distance >= 0.15)
            .OrderBy(x => x.Distance)
            .Select(x => x.Book)
            .ToList()

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

    internal static int WordOverlap(string a, string b)
    {
        var words1 = a.ToLower().Split(new char[] { ' ', ':', '-', ',', '.', ';', '?' }, StringSplitOptions.RemoveEmptyEntries);
        var words2 = b.ToLower().Split(new char[] { ' ', ':', '-', ',', '.', ';', '?' }, StringSplitOptions.RemoveEmptyEntries);
        var corr1 = CorrectQueryWords(a, words2);
        var corr1Words = corr1.Split(new char[] { ' ', ':', '-', ',', '.', ';', '?' }, StringSplitOptions.RemoveEmptyEntries);
        return corr1Words.Intersect(words2).Count();
    }

    internal static string CorrectQueryWords(string query, string[] title, int thres = 2)
    {
        var queryWords = query.Split(' ').Select(word => word.ToLower()).ToArray();

        return string.Join(" ", queryWords.Select(word =>
        {
            var best = title.Where(title => LevDistance(word, title.ToLower()) <= thres)
            .OrderBy(title => LevDistance(word, title.ToLower()))
            .FirstOrDefault();

            return best ?? word;
        }));

    }

    internal static double FinalDistance(string a, string b)
    {
        int levDist = LevDistance(a, b);
        int word = WordOverlap(a, b);

        int maxl = Math.Max(a.Length, b.Length);
        double normalized = 1 - (double)levDist / maxl;

        double combined = 0.2 * normalized + 0.8 * ((double)word / Math.Max(a.Split().Length, b.Split().Length));
        Console.WriteLine("Normilized: " + normalized);
        Console.WriteLine("Words overlap score: ", word / (double)Math.Max(a.Split().Length, b.Split().Length));
        Console.WriteLine("Combined: " + combined);
        return combined
    }


}