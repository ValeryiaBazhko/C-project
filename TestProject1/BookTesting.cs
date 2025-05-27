using Moq;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Library.Models;
using Xunit;
using Xunit.Abstractions;


namespace TestProject1;




public class BookTesting
{

    private readonly Mock<IBookRepository> _mockBookRepo;
    private readonly BookService _bookService;

    public BookTesting()
    {
        _mockBookRepo = new Mock<IBookRepository>();
        _bookService = new BookService(_mockBookRepo.Object);
    }

    
    [Theory]
    [MemberData(nameof(TestBooks))]
        public async Task GetALlBooks_ShouldReturnBooks(List<Book> books, int expected)
        {
            _mockBookRepo.Setup(repo => repo.GetAllBooks(It.IsAny<int?>(), It.IsAny<int?>())).ReturnsAsync(books);

            var result = await _bookService.GetAllBooks();
            
            Assert.Equal(expected, result.Count);
        }
    

    [Theory]
    [MemberData(nameof(TestBooks))]
    public async Task GetNumOfBooks(List<Book> books, int expected)
    {

        _mockBookRepo.Setup(repo => repo.GetAllBooks(It.IsAny<int?>(), It.IsAny<int?>())).ReturnsAsync(books);
        _mockBookRepo.Setup(repo => repo.GetTotNumOfBooks()).ReturnsAsync(expected);

        var res = await _bookService.GetTotNumOfBooks();
        Assert.Equal(expected, res);
    }

    [Theory]
    [MemberData(nameof(GetBooksSucc))]
    public async Task AddBook_ShouldAddBook(Book book)
    {
        _mockBookRepo.Setup(repo => repo.AddBook(book)).ReturnsAsync(book);

        var result = await _bookService.AddBook(book);

        Assert.NotNull(result);
        Assert.Equal(book.Id, result.Id);
        Assert.Equal(book.Title, result.Title);
    }

    [Theory]
    [MemberData(nameof(GetBooksFail))]
    public async Task AddBook_ShouldThrowException(Book book)
    {

        _mockBookRepo.Setup(repo => repo.AddBook(book)).ReturnsAsync(book);

        await Assert.ThrowsAsync<ValidationException>(async () => { await _bookService.AddBook(book); }); /////

    }

    [Theory]
    [MemberData(nameof(GetBooksSucc))]
    public async Task DeleteBook_ShouldDeleteBook(Book book)
    {
        _mockBookRepo.Setup(repo => repo.DeleteBook(It.IsAny<int>())).ReturnsAsync(true);

        Assert.True(await _bookService.DeleteBook(book.Id));

        _mockBookRepo.Verify(repo => repo.DeleteBook(It.IsAny<int>()), Times.Once);
    }

    [Theory]
    [MemberData(nameof(GetBooksSucc))]
    public async Task DeleteBook_ShouldThrowException(Book book)
    {
        _mockBookRepo.Setup(repo => repo.DeleteBook(It.IsAny<int>())).ReturnsAsync(false);

        Assert.False(await _bookService.DeleteBook(int.MaxValue));
    }

    [Theory]
    [MemberData(nameof(GetBooksSucc))]
    public async Task GetBookById_ShouldReturnBook(Book book)
    {
        _mockBookRepo.Setup(repo => repo.GetBookById(book.Id)).ReturnsAsync(book);
        
        var result = await _bookService.GetBookById(book.Id);
        Assert.NotNull(result);
        Assert.Equal(book.Id, result.Id);
    }

    [Theory]
    [MemberData(nameof(GetBooksFail))]
    public async Task GetBookById_ShouldFail(Book book)
    {
        
        _mockBookRepo.Setup(repo => repo.GetBookById(book.Id)).ReturnsAsync(book);
        
        await Assert.ThrowsAsync<ValidationException>(() => _bookService.GetBookById(book.Id));
    }


    [Theory]
    [MemberData(nameof(GetBooksSucc))]
    public async Task UpdateBook_ShouldUpdateBook(Book book)
    {
        _mockBookRepo.Setup(repo=>repo.UpdateBook(book)).ReturnsAsync(true);
        
        await  _bookService.UpdateBook(book);
    }
    
    
    [Theory]
    [MemberData(nameof(GetBooksFail))]
    public async Task UpdateBook_ShouldThrowException(Book book)
    {
        _mockBookRepo.Setup(repo => repo.UpdateBook(book)).ReturnsAsync(false);
        
        await Assert.ThrowsAsync<ValidationException>(() => _bookService.UpdateBook(book));
    }

    [Theory]
    [MemberData(nameof(TestBooks))]
    public async Task SimilaritySearch_Trivial_ShouldReturnBooks(List<Book> books, int expected)
    {
        
        Assert.NotNull(books);

        _mockBookRepo.Setup(repo => repo.GetAllBooks(It.IsAny<int?>(), It.IsAny<int?>())).ReturnsAsync(books);
        var result1 = await _bookService.SearchBooks("");
        var result2 = await _bookService.SearchBooks("red");
        
        Assert.Equal(expected, result1.Count);
        Assert.Empty(result2);
    }
    
    
    public static List<object[]> GetBooksFail()
    {
        return new List<object[]>
        {
            new object[] { new Book { Id = -1, Title = "", PublicationYear = 2021, AuthorId = 123 } },
            new object[] { new Book { Id = -2, Title = "Book 2", PublicationYear = 2055, AuthorId = 123 }}
        };

    }
    
    public static List<object[]> GetBooksSucc()
    {
        return new List<object[]>
        {
            new object[] { new Book { Id = 1, Title = "Book 1", PublicationYear = 2021, AuthorId = 123 } },
            new object[] { new Book { Id = 2, Title = "Book 2", PublicationYear = 2000, AuthorId = 123 }}
        };

    }

    public static List<object[]> TestBooks()
    {
        return new List<object[]>
        {
            new object[]
            {
                new List<Book>
                {
                    new Book { Id = 1, Title = "Test", PublicationYear = 2021, AuthorId = 123 }
                },
                1
            },
            new object[]
            {
                new List<Book>
                {
                    new Book { Id = 1, Title = "Book 1", PublicationYear = 2021, AuthorId = 123 },
                    new Book { Id = 2, Title = "Book 2", PublicationYear = 2025, AuthorId = 123 }
                },
                2
            },
            new object[]
            {
                new List<Book>(),
                0
            } 
        };
    }
}