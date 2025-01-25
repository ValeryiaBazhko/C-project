namespace Library.Models

public class Book
{
    public Author? Author { get; set; }
    public int Id { get; set; }
    public string Title { get; set; }
    public int PublicationYear { get; set; }
    public int AuthorId { get; set}

}