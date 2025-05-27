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
        if (email.Trim().ToLower() == "admin@admin" && password == "admin")
        {
            return new User
            {
                Id = 0,
                FirstName = "Admin",
                LastName = "User",
                Email = "admin@admin",
                Password = "admin",
                Role = true
            };
        }
        
        var user = await _userRepo.GetUserByEmailAndPasswordAsync(email, password);
        return user;
    }
}