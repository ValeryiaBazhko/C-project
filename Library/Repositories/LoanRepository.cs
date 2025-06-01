using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Models;
using Microsoft.EntityFrameworkCore;

public class LoanRepository : ILoanRepository
{
    private readonly LibraryContext _context;

    public LoanRepository(LibraryContext context)
    {
        _context = context;
    }

    public async Task<List<Loan>> GetAllLoans(int? pageNum = null, int? pageSize = null)
    {
        IQueryable<Loan> loans = _context.Loans
            .Include(l => l.User)
            .Include(l => l.Book)
            .OrderByDescending(l => l.CheckoutDate);

        if (pageNum.HasValue && pageSize.HasValue && pageNum > 0 && pageSize > 0)
        {
            loans = loans.Skip((pageNum.Value - 1) * pageSize.Value).Take(pageSize.Value);
        }

        return await loans.ToListAsync() ?? new List<Loan>();
    }

    public async Task<Loan?> GetLoanById(int id)
    {
        return await _context.Loans
            .Include(l => l.User)
            .Include(l => l.Book)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<Loan> AddLoan(Loan loan)
    {
        _context.Loans.Add(loan);
        await _context.SaveChangesAsync();
        
        return await GetLoanById(loan.Id) ?? loan;
    }

    public async Task<bool> UpdateLoan(Loan loan)
    {
        var existingLoan = await _context.Loans.FindAsync(loan.Id);
        if (existingLoan == null) return false;
        
        existingLoan.Status = loan.Status;
        existingLoan.ReturnDate = loan.ReturnDate?.ToUniversalTime();
        
        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Update error: {ex.InnerException?.Message}");
            throw;
        }
    }

    public async Task<bool> DeleteLoan(int id)
    {
        var loan = await _context.Loans.FindAsync(id);
        if (loan == null) return false;

        _context.Loans.Remove(loan);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int> GetTotNumOfLoans()
    {
        return await _context.Loans.CountAsync();
    }

    public async Task<List<Loan>> GetLoansByUserId(int userId)
    {
        return await _context.Loans
            .Include(l => l.User)
            .Include(l => l.Book)
            .Where(l => l.User.Id == userId)
            .OrderByDescending(l => l.CheckoutDate)
            .ToListAsync();
    }

    public async Task<List<Loan>> GetLoansByBookId(int bookId)
    {
        return await _context.Loans
            .Include(l => l.User)
            .Include(l => l.Book)
            .Where(l => l.Book.Id == bookId)
            .OrderByDescending(l => l.CheckoutDate)
            .ToListAsync();
    }

    public async Task<List<Loan>> GetLoansByStatus(string status)
    {
        return await _context.Loans
            .Include(l => l.User)
            .Include(l => l.Book)
            .Where(l => l.Status.ToLower() == status.ToLower())
            .OrderByDescending(l => l.CheckoutDate)
            .ToListAsync();
    }

    public async Task<List<Loan>> GetOverdueLoans()
    {
        var today = DateTime.Now;
        return await _context.Loans
            .Include(l => l.User)
            .Include(l => l.Book)
            .Where(l => l.DueDate < today && l.Status.ToLower() != "returned")
            .OrderBy(l => l.DueDate)
            .ToListAsync();
    }
    
    
}