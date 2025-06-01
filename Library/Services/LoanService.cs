using System;
using System.Collections.Generic;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

public class LoanService
{
    private readonly ILoanRepository _loanRepository;
    private readonly LibraryContext _context; // Add context for fetching entities

    public LoanService(ILoanRepository loanRepository, LibraryContext context)
    {
        _loanRepository = loanRepository;
        _context = context;
    }

    public async Task<List<Loan>> GetAllLoans(int? pageNum = null, int? pageSize = null)
    {
        return await _loanRepository.GetAllLoans(pageNum, pageSize);
    }

    public async Task<int> GetTotNumOfLoans()
    {
        return await _loanRepository.GetTotNumOfLoans();
    }

    public async Task<Loan?> GetLoanById(int id)
    {
        if (id < 0)
        {
            throw new ValidationException("Invalid ID");
        }
        
        var loan = await _loanRepository.GetLoanById(id);
        return loan;
    }

    public async Task<Loan> AddLoan(Loan loan)
    {
        // Validate IDs first
        if (loan.UserId <= 0)
        {
            throw new ValidationException("Valid User ID is required");
        }
        
        if (loan.BookId <= 0)
        {
            throw new ValidationException("Valid Book ID is required");
        }

        // Check if entities exist
        var user = await _context.Users.FindAsync(loan.UserId);
        if (user == null)
        {
            throw new ValidationException($"User with ID {loan.UserId} not found");
        }

        var book = await _context.Books.FindAsync(loan.BookId);
        if (book == null)
        {
            throw new ValidationException($"Book with ID {loan.BookId} not found");
        }

        // Set defaults
        if (loan.CheckoutDate == default)
        {
            loan.CheckoutDate = DateTime.Now;
        }
        
        if (loan.FromDate == default)
        {
            loan.FromDate = loan.CheckoutDate;
        }
        
        if (loan.DueDate == default)
        {
            loan.DueDate = loan.CheckoutDate.AddDays(14);
        }
        
        if (string.IsNullOrWhiteSpace(loan.Status))
        {
            loan.Status = "Active";
        }
        
        if (loan.FromDate > loan.DueDate)
        {
            throw new ValidationException("From date cannot be after due date");
        }

        var validStatuses = new[] { "Active", "Returned", "Overdue", "Renewed" };
        if (!validStatuses.Contains(loan.Status, StringComparer.OrdinalIgnoreCase))
        {
            throw new ValidationException($"Status must be one of: {string.Join(", ", validStatuses)}");
        }

        await _loanRepository.AddLoan(loan);
        return loan;
    }

    public async Task<bool> UpdateLoan(Loan loan)
    {
        ValidateLoan(loan);
        return await _loanRepository.UpdateLoan(loan);
    }

    public async Task<bool> DeleteLoan(int id)
    {
        if (id < 0)
        {
            throw new ValidationException("Invalid ID");
        }
        return await _loanRepository.DeleteLoan(id);
    }

    public async Task<List<Loan>> GetLoansByUserId(int userId)
    {
        if (userId < 0)
        {
            throw new ValidationException("Invalid User ID");
        }
        return await _loanRepository.GetLoansByUserId(userId);
    }

    public async Task<List<Loan>> GetLoansByBookId(int bookId)
    {
        if (bookId < 0)
        {
            throw new ValidationException("Invalid Book ID");
        }
        return await _loanRepository.GetLoansByBookId(bookId);
    }

    public async Task<List<Loan>> GetLoansByStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            throw new ValidationException("Status cannot be empty");
        }
        return await _loanRepository.GetLoansByStatus(status);
    }

    public async Task<List<Loan>> GetOverdueLoans()
    {
        return await _loanRepository.GetOverdueLoans();
    }

    public async Task<bool> ReturnLoan(int loanId)
    {
        var loan = await _context.Loans.FindAsync(loanId);
        if (loan == null)
        {
            throw new ValidationException("Loan not found");
        }
        
        loan.Status = "Returned";
        loan.ReturnDate = DateTime.UtcNow; 

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error saving loan: {ex.InnerException?.Message}");
            throw new ValidationException("Error updating loan in database");
        }
    }

    private void ValidateLoan(Loan loan)
    {
        if (loan.User == null)
        {
            throw new ValidationException("User is required");
        }

        if (loan.Book == null)
        {
            throw new ValidationException("Book is required");
        }

        if (string.IsNullOrWhiteSpace(loan.Status))
        {
            throw new ValidationException("Status cannot be empty");
        }

        var validStatuses = new[] { "Active", "Returned", "Overdue", "Renewed" };
        if (!validStatuses.Contains(loan.Status, StringComparer.OrdinalIgnoreCase))
        {
            throw new ValidationException($"Status must be one of: {string.Join(", ", validStatuses)}");
        }

        if (loan.FromDate > loan.DueDate)
        {
            throw new ValidationException("From date cannot be after due date");
        }

        if (loan.CheckoutDate > DateTime.Now.AddDays(1))
        {
            throw new ValidationException("Checkout date cannot be in the future");
        }
    }
}