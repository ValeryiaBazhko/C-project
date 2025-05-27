using Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;

    public AuthController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
    {
        var user = await _userService.AuthenticateAsync(loginDto.Email, loginDto.Password);

        if (user == null)
            return Unauthorized("Invalid email or password.");
        
        return Ok(new {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role
        });
    }
}
