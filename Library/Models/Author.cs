using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Newtonsoft.Json;

namespace Library.Models;


public class Author
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name is too long")]
    public required string Name { get; set; }
    public DateOnly DateOfBirth { get; set; }

    public List<Book>? Books { get; set; } = new List<Book>();


   
    
    
}