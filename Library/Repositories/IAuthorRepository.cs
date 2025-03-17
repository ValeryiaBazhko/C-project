using Library.Models;

public interface IAuthorRepository
{
    Task<List<Author>> GetAllAuthors();
    Task<Author?> GetAuthorById(int id);
    Task<Author> AddAuthor(Author author);

}