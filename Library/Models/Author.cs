using System.ComponentModel.DataAnnotations;

namespace Library.Models;


public class Author
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name is too long")]
    public required string Name { get; set; }

    
    public DateTime DateOfBirth { get; set; }

    public List<Book>? Books { get; set; } = new List<Book>();
}

/* public class ValidDate : ValidationAttribute
     {
         public override bool IsValid(object value)
         {
             if (value is DateTime date)
             {
                 int year = date.Year;
                 if (year < 1000 || year > DateTime.Now.Year){return false;}
                 
                }return true;
         }
     }
     */