using Library.Models;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Library.Tests;

[TestClass]
public class UnitTest1
{
    private Mock<IBookRepository> _mockBookRepo;
    private BookService _bookService;

    [TestInitialize]
    public void TestInitialize() {
        _mockBookRepo = new Mock<IBookRepository>();
        _bookService = new BookService(_mockBookRepo.Object);
    }

    [TestMethod]
    public async Task GetALlBooks_ShouldReturnBooks() {
        var books = new List<Book> {
            new Book { Id = 1, Title = "Book 1", PublicationYear = 2020, AuthorId = 123},
            new Book { Id = 2, Title = "Book 2", PublicationYear = 2021, AuthorId = 123}
        };
        _mockBookRepo.Setup(repo => repo.GetAllBooks(It.IsAny<int?>(), It.IsAny<int?>())).ReturnsAsync(books);


        var result = await _bookService.GetAllBooks();

        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("Book 1", result[0].Title);
    }

    [TestMethod]
    public async Task AddBook_InCorrectly() {
        var book = new Book {
            Id = 15,
            Title = "",
            PublicationYear = 2055,
            AuthorId = 123
        };
        _mockBookRepo.Setup(repo => repo.AddBook(book)).ReturnsAsync(book);

        await Assert.ThrowsExceptionAsync<ValidationException>(async () => 
        {
            await _bookService.AddBook(book);
        });
}

    [TestMethod]
    public async Task AddBook_Correctly() {
        var book = new Book {
            Id = 33,
            Title = "Red Apple",
            PublicationYear = 2023,
            AuthorId = 123
        };

        _mockBookRepo.Setup(repo => repo.AddBook(book)).ReturnsAsync(book);

        var result = await _bookService.AddBook(book);
        
        Assert.IsNotNull(result);
        Assert.AreEqual("Red Apple", result.Title);
        Assert.AreEqual(2023, result.PublicationYear);
    }
}