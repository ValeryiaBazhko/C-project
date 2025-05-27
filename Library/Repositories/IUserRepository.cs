using Library.Models;

namespace Library.Repositories;


    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAndPasswordAsync(string email, string password);
        
        Task<User?> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
    }
    