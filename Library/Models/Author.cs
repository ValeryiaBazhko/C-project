namespace Library.Models

public class Author
{

    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime DateOfBirth { get; set }

    public List<Book>? Books { get; set; }

}