using Library.Models;
using Library.Repositories;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly LibraryContext _context;

    public UserRepository(LibraryContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByEmailAndPasswordAsync(string email, string password)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
    }
}