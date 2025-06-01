using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models;

public class Loan
{
    public int Id { get; set; }
    
    public string Status { get; set; }
    
    [Column(TypeName = "timestamp with time zone")]
    public DateTime FromDate { get; set; }
    
    [Column(TypeName = "timestamp with time zone")]
    public DateTime DueDate { get; set; }
    
    [Column(TypeName = "timestamp with time zone")]
    public DateTime? ReturnDate { get; set; }
    
    [Column(TypeName = "timestamp with time zone")]
    public DateTime CheckoutDate { get; set; }
    
    public User User { get; set; }
    public int UserId { get; set; }
    public int BookId { get; set; }
    
    public Book Book { get; set; }
}