using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Models;

public interface ILoanRepository
{
    Task<List<Loan>> GetAllLoans(int? pageNum = null, int? pageSize = null);
    Task<Loan?> GetLoanById(int id);
    Task<Loan> AddLoan(Loan loan);
    Task<bool> UpdateLoan(Loan loan);
    Task<bool> DeleteLoan(int id);
    Task<int> GetTotNumOfLoans();
    Task<List<Loan>> GetLoansByUserId(int userId);
    Task<List<Loan>> GetLoansByBookId(int bookId);
    Task<List<Loan>> GetLoansByStatus(string status);
    Task<List<Loan>> GetOverdueLoans();
}