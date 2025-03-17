using Library.Models;
using Microsoft.EntityFrameworkCore;

public class AuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<List<Author>> GetAllAuthors()
    {
        return await _authorRepository.GetAllAuthors();
    }

    public async Task<Author?> GetAuthorById(int id)
    {
        return await _authorRepository.GetAuthorById(id);
    }

    public async Task<Author> AddAuthor(Author author)
    {
        return await _authorRepository.AddAuthor(author);
    }
}