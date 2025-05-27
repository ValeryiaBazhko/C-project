using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Library.Models;
using Microsoft.EntityFrameworkCore;

public class AuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<List<Author>> GetAllAuthors() ////
    {
        return await _authorRepository.GetAllAuthors();
    }

    public async Task<Author?> GetAuthorById(int id) ////
    {
        if (id < 0)
        {
            throw new ValidationException("Invalid ID");
        }

        var author = await _authorRepository.GetAuthorById(id);
        return author;
    }

    public async Task<Author> AddAuthor(Author author) ///
    {
        
        if (author.Id < 0)
        {
            throw new ValidationException("Invalid ID");
        }

        if (author.DateOfBirth > DateOnly.FromDateTime(DateTime.Now))
        {
            throw new ValidationException("Invalid Date");
        }

        if (string.IsNullOrWhiteSpace(author.Name))
        {
            throw new ValidationException("Invalid Name");
        }
        
        await _authorRepository.AddAuthor(author);
        return author;
    }
}