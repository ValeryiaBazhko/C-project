using System;

namespace Library.Models
{
    public class LoanCreateDto
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public string Status { get; set; } = "Active";
        public DateTime FromDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CheckoutDate { get; set; }
    }
}