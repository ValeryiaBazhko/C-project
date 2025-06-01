using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library.Models;
using System.ComponentModel.DataAnnotations;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly LoanService _loanService;

        public LoansController(LoanService loanService)
        {
            _loanService = loanService;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Loan>>> GetLoans([FromQuery] int? pageNum, [FromQuery] int? pageSize)
        {
            if (!pageNum.HasValue || !pageSize.HasValue || pageNum <= 0 || pageSize <= 0)
            {
                return await _loanService.GetAllLoans();
            }

            var loans = await _loanService.GetAllLoans(pageNum, pageSize);
            var totalLoans = await _loanService.GetTotNumOfLoans();

            var output = new
            {
                PageNumber = pageNum.Value,
                PageSize = pageSize.Value,
                TotalLoans = totalLoans,
                TotalPages = (totalLoans % pageSize == 0) ? (totalLoans / pageSize) : (totalLoans / pageSize) + 1,
                Loans = loans
            };

            return Ok(output);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Loan>> GetLoanById(int id)
        {
            try
            {
                var loan = await _loanService.GetLoanById(id);
                if (loan == null) return NotFound();
                return loan;
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Loan>>> GetLoansByUserId(int userId)
        {
            try
            {
                var loans = await _loanService.GetLoansByUserId(userId);
                return Ok(loans);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("book/{bookId}")]
        public async Task<ActionResult<IEnumerable<Loan>>> GetLoansByBookId(int bookId)
        {
            try
            {
                var loans = await _loanService.GetLoansByBookId(bookId);
                return Ok(loans);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<Loan>>> GetLoansByStatus(string status)
        {
            try
            {
                var loans = await _loanService.GetLoansByStatus(status);
                return Ok(loans);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("overdue")]
        public async Task<ActionResult<IEnumerable<Loan>>> GetOverdueLoans()
        {
            var loans = await _loanService.GetOverdueLoans();
            return Ok(loans);
        }

        [HttpPost]
        public async Task<ActionResult<Loan>> PostLoan([FromBody] LoanCreateDto loanDto)
        {
            try
            {
                var loan = new Loan
                {
                    UserId = loanDto.UserId,
                    BookId = loanDto.BookId,
                    Status = loanDto.Status,
                    FromDate = loanDto.FromDate,
                    DueDate = loanDto.DueDate,
                    CheckoutDate = loanDto.CheckoutDate,
                    ReturnDate = DateTime.MinValue 
                };

                var createdLoan = await _loanService.AddLoan(loan);
                return CreatedAtAction(nameof(GetLoanById), new { id = createdLoan.Id }, createdLoan);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Loans/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoan(int id, Loan loan)
        {
            if (id != loan.Id)
            {
                return BadRequest("Incorrect ID");
            }

            try
            {
                var result = await _loanService.UpdateLoan(loan);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Loans/5/return
        [HttpPut("{id}/return")]
        public async Task<IActionResult> ReturnLoan(int id)
        {
            try
            {
                var result = await _loanService.ReturnLoan(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { 
                    message = "Database error while returning loan",
                    details = ex.InnerException?.Message 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    message = "Unexpected error while returning loan",
                    details = ex.Message 
                });
            }
        }

        // DELETE: api/Loans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            try
            {
                var result = await _loanService.DeleteLoan(id);
                if (!result) return NotFound();
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}