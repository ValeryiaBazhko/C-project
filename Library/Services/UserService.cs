using Library.Models;
using Library.Repositories;

public class UserService
{
    private readonly IUserRepository _userRepo;

    public UserService(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<User?> AuthenticateAsync(string email, string password)
    {
        var user = await _userRepo.GetUserByEmailAsync(email);
    
        if (user == null)
        {
            Console.WriteLine($"User not found: {email}");
            return null;
        }
        
        
        if (user.Password == password)
        {
            return user;
        }
        
        if (BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return user;
        }
        
        return null;
    }
    
    public async Task<bool> UserExistsAsync(string email)
    {
        return await _userRepo.GetUserByEmailAsync(email) != null;
    }

    public async Task CreateUserAsync(User user)
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        await _userRepo.AddUserAsync(user);
    }
}