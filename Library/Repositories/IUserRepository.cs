using Library.Models;

namespace Library.Repositories;


    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAndPasswordAsync(string email, string password);
    }
    