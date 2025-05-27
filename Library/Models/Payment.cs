using System;

namespace Library.Models;

public class Payment
{
    public int Id { get; set; }
    
    public int Amount { get; set; }
    
    public string Status { get; set; }
    
    public DateTime IssueDate { get; set; }
    
    public DateTime PaymentDate { get; set; }
    
    public Loan Loan { get; set; }
}