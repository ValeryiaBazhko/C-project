using System;
using System.Threading.Tasks;
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
    
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        Console.WriteLine(user.Role);
        return user;
    }

    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}