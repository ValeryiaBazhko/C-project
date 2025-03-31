using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using FluentAssertions;
using Library;
using Library.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace Library.Api.IntegrationTests;

public class LibraryControllersTest : IDisposable
{
    private readonly LibraryContext _context;
    private readonly BookService _bookService;
    public LibraryControllersTest()
    {
        var connString = GetConnectionString();
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseNpgsql(connString)
            .Options;
        
        _context = new LibraryContext(options);
        _context.Database.EnsureDeleted();
        _context.Database.Migrate();
        
        var bookRepository = new BookRepository(_context);
        _bookService = new BookService(bookRepository);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
    
    [Fact]
    public async Task GetBooks_ShouldReturnBooks()
    {
        
        
        _context.Authors.AddRange(Enumerable.Range(1,5).Select(x => new Author
        {
            Name = $"Author {x}",
            DateOfBirth = DateOnly.FromDateTime(DateTime.Now.AddYears(-x))
        }));

        await _context.SaveChangesAsync();

        foreach (var x in Enumerable.Range(1, 5))
        {
            var validBook = new Book
            {
                Title = $"Book {x}",
                PublicationYear = 1990 + x,
                AuthorId = x
            };
            await _bookService.AddBook(validBook);
        }

        var invalidBooks = new List<Book>
        {
            new Book { Title = "", PublicationYear = 1990, AuthorId = 1 },
            new Book { Title = "Invalid cause of year", PublicationYear = 2044, AuthorId = 1 },
        };
        
        var results = new List<Exception>();

        foreach (var x in invalidBooks)
        {
            try
            {
                await _bookService.AddBook(x);
            }
            catch(Exception ex)
            {
                results.Add(ex);
            }
        }
        
        results.Should().HaveCount(invalidBooks.Count);
        await _context.SaveChangesAsync();
        var books = await _bookService.GetAllBooks();
        books.Should().HaveCount(5);
        books.Should().Contain(b => b.Title == "Book 1");
    }
    
    
    [Fact]
    public async Task GetTotNumOfBooks_ShouldReturnNumOfBooks()
    {
        _context.Authors.AddRange(Enumerable.Range(1,5).Select(x => new Author
        {
            Name = $"Author {x}",
            DateOfBirth = DateOnly.FromDateTime(DateTime.Now.AddYears(-x))
        }));

        await _context.SaveChangesAsync();

        foreach (var x in Enumerable.Range(1, 5))
        {
            var validBook = new Book
            {
                Title = $"Book {x}",
                PublicationYear = 1990 + x,
                AuthorId = x
            };
            await _bookService.AddBook(validBook);
        }

        var invalidBooks = new List<Book>
        {
            new Book { Title = "", PublicationYear = 1990, AuthorId = 1 },
            new Book { Title = "Invalid cause of year", PublicationYear = 2044, AuthorId = 1 },
        };
        
        var results = new List<Exception>();

        foreach (var x in invalidBooks)
        {
            try
            {
                await _bookService.AddBook(x);
            }
            catch(Exception ex)
            {
                results.Add(ex);
            }
        }
        
        results.Should().HaveCount(invalidBooks.Count);
        await _context.SaveChangesAsync();
        var books = await _bookService.GetTotNumOfBooks();
        books.Should().BePositive();
        books.Should().Be(5);
    }

    [Fact]
    public async Task GetBookById_ShouldReturnBook()
    {
        _context.Authors.Add(new Author
        {
            Name = "Author 1",
            DateOfBirth = DateOnly.FromDateTime(DateTime.Now.AddYears(-100))
        });
        await _context.SaveChangesAsync();
        _context.Books.AddRange(Enumerable.Range(1, 5).Select(x => new Book
        {
            Title = $"Book {x}",
            PublicationYear = 1990 + x,
            AuthorId = 1
        }));
        await _context.SaveChangesAsync();
        
        var book =  await _bookService.GetBookById(1);
        book.Should().NotBeNull();
        book.Title.Should().Be("Book 1");
        book.PublicationYear.Should().Be(1991);
    }

    [Fact]
    public async Task AddBook_ShouldAddBook()
    {
        _context.Authors.Add(new Author
        {
            Name = "Author 1",
            DateOfBirth = DateOnly.FromDateTime(DateTime.Now.AddYears(-100))
        });
        
        await _context.SaveChangesAsync();

        var book1 = new Book
        {
            Title = "Book 1",
            PublicationYear = 1990,
            AuthorId = 1
        };
        
        var book2 = new Book
        {
            Title = "",
            PublicationYear = 1990,
            AuthorId = 1
        };
        
        var res1 = await _bookService.AddBook(book1);
        var res2 = await Assert.ThrowsAsync<ValidationException>(async () => await _bookService.AddBook(book2));

        res1.Should().NotBeNull();
        res1.Should().BeSameAs(book1);
        res2.Message.Should().Be("Title cannot be empty");
    }

    [Fact]
    public async Task UpdateBook_ShouldUpdateBook()
    {
        _context.Authors.Add(new Author
        {
            Name = "Author 1",
            DateOfBirth = DateOnly.FromDateTime(DateTime.Now.AddYears(-100))
        });
        
        _context.Books.AddRange(Enumerable.Range(1, 5).Select(x => new Book
        {
            Title = $"Book {x}",
            PublicationYear = 1990 + x,
            AuthorId = 1
        }));
        
        await _context.SaveChangesAsync();

        var booktoup = await _context.Books.FirstOrDefaultAsync();
        booktoup.Should().NotBeNull();
        booktoup.Title.Should().Be("Book 1");
        
        booktoup.Title =  "Book 11";
        booktoup.PublicationYear = 1975;
        
        var res =  await _bookService.UpdateBook(booktoup);

        res.Should().BeTrue();
        
        var updbook = await _context.Books.FindAsync(booktoup.Id);
        updbook.Should().NotBeNull();
        updbook.Title.Should().Be("Book 11");
        updbook.PublicationYear.Should().Be(1975);
    }

    [Fact]
    public async Task DeleteBook_ShouldDeleteBook()
    {
        _context.Authors.Add(new Author
        {
            Name = "Author 1",
            DateOfBirth = DateOnly.FromDateTime(DateTime.Now.AddYears(-100))
        });
        
        _context.Books.AddRange(Enumerable.Range(1,5).Select(x => new Book
        {
            Title = $"Book {x}",
            PublicationYear = 1990 + x,
            AuthorId = 1
        }));
        
        await _context.SaveChangesAsync();
        
        var res1 = await _bookService.DeleteBook(1);
        var res2 = await _bookService.DeleteBook(7);
        
        res1.Should().BeTrue();
        res2.Should().BeFalse();
    }

    public async Task SearchBooks_ShouldReturnBooks(string query)
    {
        _context.Authors.Add(new Author
        {
            Name = "Author 1",
            DateOfBirth = DateOnly.FromDateTime(DateTime.Now.AddYears(-100))
        });

        _context.Books.Add(new Book
        {
            Title = "Harry Potter",
            PublicationYear = 1990,
            AuthorId = 1
        });

        _context.Books.Add(new Book
        {
            Title = "Green Apple",
            PublicationYear = 1990,
            AuthorId = 1
        });
        
        _context.Books.Add(new Book
        {
            Title = "Red Apple",
            PublicationYear = 1990,
            AuthorId = 1
        });
        
        await _context.SaveChangesAsync();
        
        var res1 = await _bookService.SearchBooks("apple");
        var res2 = await _bookService.SearchBooks("green");
        var res3 = await _bookService.SearchBooks("harry");
        var res4 = await _bookService.SearchBooks("pie");
        
        res1.Count().Should().Be(2);
        res2.Count().Should().Be(1);
        res3.Count().Should().Be(1);
        res4.Count().Should().Be(0);
    }
    
    
    private static string? GetConnectionString()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<LibraryControllersTest>()
            .Build();
        
        var connectionString = configuration.GetConnectionString("Library");
        return connectionString;
    }
}