using Library.Models;
using Microsoft.CodeAnalysis.Elfie.Model.Tree;
using Moq;

namespace TestProject1;

public class SimilaritySearchChecking
{
    private readonly Mock<IBookRepository> _mockBookRepo;
    private readonly BookService _bookService;

    public SimilaritySearchChecking()
    {
        _mockBookRepo = new Mock<IBookRepository>();
        _bookService = new BookService(_mockBookRepo.Object);
    }

    [Theory]
    [InlineData("red", "", 3)]
    [InlineData("red", "ready", 2)]
    [InlineData("", "",0 )]
    [InlineData("red", "red", 0)]
    [InlineData("apple", "Aple", 2)]
    [InlineData("green", "ready", 4)]
    public async Task LeviDist_Calc(string str1, string str2, int expected)
    {
        int result = BookService.LevDistance(str1, str2);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Harry Potter", "Potter Scary", 1)]
    [InlineData("Red Apple", "Harry Potter", 0)]
    [InlineData("", "red apple", 0)]
    [InlineData("Harry Potter Apple", "Harry Potter Apple", 3)]
    public async Task WordOverlap_Calc(string str1, string str2, int expected)
    {
        int result = BookService.WordOverlap(str1, str2);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Hary Poter", new string[] { "Hary", "Potter" },  "Harry Potter" )]
    [InlineData("Aple Pei", new string[] {"Apple", "Pie"}, "Apple Pie")]
    public async Task CorrectQueryWords(string str1, string[] str2, string expected)
    {
        
    }

}