using System;

namespace Library.Models;

public class Loan
{
    public int Id { get; set; }
    
    public string Status { get; set; }
    
    public DateTime FromDate { get; set; }
    
    public DateTime DueDate { get; set; }
    
    public DateTime? ReturnDate { get; set; }
    
    public DateTime CheckoutDate { get; set; }
    
    public User User { get; set; }
    public int UserId { get; set; }
    public int BookId { get; set; }
    
    public Book Book { get; set; }
}