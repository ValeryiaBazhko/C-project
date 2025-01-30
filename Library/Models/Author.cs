using System.ComponentModel.DataAnnotations;

namespace Library.Models;


public class Author
{
    [Book.ValidId(ErrorMessage = "Invalid Id")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name is too long")]
    public string Name { get; set; }

    public DateTime DateOfBirth { get; set; }

    public List<Book>? Books { get; set; } = new List<Book>();

}
