using System.ComponentModel.DataAnnotations;

namespace Library.Models;

public class UserRegDto
{
    [Required] 
    public string FirstName { get; set; } = null!;

    [Required] 
    public string LastName { get; set; } = null!;
    
    [Required]
    [MinLength(6)]
    public string Password { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }


    public bool Role { get; set; } = false;
}