using System.ComponentModel.DataAnnotations;
using Library.Models;
using Moq;

namespace TestProject1;

public class AuthorTesting
{
    private readonly Mock<IAuthorRepository> _mockAuthorRepo;
    private readonly AuthorService _authorService;

    public AuthorTesting()
    {
        _mockAuthorRepo = new Mock<IAuthorRepository>();
        _authorService = new AuthorService(_mockAuthorRepo.Object);
    }
    
    [Theory]
    [MemberData(nameof(TestAuthors))]
    public async Task GetAllAuthors_ShouldReturnAuthors(List<Author> authors, int expected)
    {
        _mockAuthorRepo.Setup(repo => repo.GetAllAuthors()).ReturnsAsync(authors);
        
        var result = await _authorService.GetAllAuthors();

        Assert.Equal(expected, result.Count);
    }

    [Theory]
    [MemberData(nameof(GetAuthorSucc))]
    public async Task GetAuthorById_ShouldReturnAuthor(Author author)
    {
        _mockAuthorRepo.Setup(repo => repo.GetAuthorById(author.Id)).ReturnsAsync(author);
        var result = await _authorService.GetAuthorById(author.Id);

        Assert.NotNull(result);
        Assert.Equal(author.Id, result.Id);
    }

    [Theory]
    [MemberData(nameof(GetAuthorFail))]
    public async Task GetAuthorById_ShouldThrowException(Author author)
    {
        _mockAuthorRepo.Setup(repo => repo.GetAuthorById(author.Id)).ReturnsAsync(author);
        
        await Assert.ThrowsAsync<ValidationException>(() => _authorService.GetAuthorById(author.Id));
    }


    [Theory]
    [MemberData(nameof(GetAuthorSucc))]
    public async Task AddAuthor_ShouldAddAuthor(Author author)
    {
        _mockAuthorRepo.Setup(repo => repo.AddAuthor(author)).ReturnsAsync(author);
        
        var result = await _authorService.AddAuthor(author);

        Assert.NotNull(result);
        Assert.Equal(author.Id, result.Id);
    }

    [Theory]
    [MemberData(nameof(GetAuthorFail))]
    public async Task AddAuthor_ShouldThrowException(Author author)
    {
        _mockAuthorRepo.Setup(repo => repo.AddAuthor(author)).ReturnsAsync(author);
        
        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await _authorService.AddAuthor(author);
        });
    }







    public static List<object[]> GetAuthorSucc()
    {
        return new List<object[]>
        {
            new object[] { new Author { Id = 1, DateOfBirth = new DateOnly(2004, 12, 12), Name = "someone" } },
            new object[] { new Author { Id = 2, DateOfBirth = new DateOnly(2000, 07, 12), Name = "someone else" } }
        };
    }

    public static List<object[]> GetAuthorFail()
    {
        return new List<object[]>
        {
            new object[] { new Author { Id = -1, DateOfBirth = new DateOnly(2023, 12, 03), Name = "someone" } },
            new object[] { new Author { Id = -2, DateOfBirth = new DateOnly(2055, 08,23), Name = "" } },
        };
    }



    public static List<object[]> TestAuthors()
    {
        return new List<object[]>
        {
            new object[]
            {
                new List<Author>
                {
                    new Author { Id = 1, DateOfBirth = new DateOnly(2004, 07, 12), Name = "Someone" },
                    new Author { Id = 2, DateOfBirth = new DateOnly(2004, 07, 22), Name = "Someone else" }
                },
                2
            },
            new object[]
            {
                new List<Author>
                {
                    new Author { Id = 1, DateOfBirth = new DateOnly(2004, 07, 22), Name = "Someone" }
                },
                1
            },
            new object[]
            {
                new List<Author>(),
                0
            }
        };
    }

}