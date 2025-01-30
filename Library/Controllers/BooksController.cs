using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library.Models;
using System.Numerics;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks([FromQuery] int pageNum, [FromQuery] int pageSize)
        {
            var books = await _bookService.GetAllBooks(pageNum, pageSize);
            var totBooks = await _bookService.GetTotNumOfBooks();

            var Out = new
            {
                PageNumber = pageNum,
                PageSize = pageSize,
                TotalBooks = totBooks,
                TotalPages = (int)(totBooks / pageSize),
                Books = books
            };

            return Ok(Out);
        }


        // GET /books/search?query=<query>
        // modify to search query 
        [HttpGet("search")]
        public async Task<ActionResult<Book>> SearchBooksByTitle(string query)
        {
            if (string.IsNullOrEmpty(query)) { return BadRequest("Provide a title of the book"); }

            var books = await _bookService.SearchBooks(query);

            if (books == null || books.Count == 0)
            {
                return NotFound("No books found");
            }

            return Ok(books);
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest("Incorrect ID");
            }

            var Out = await _bookService.UpdateBook(book);

            if (!Out)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {

            var author = await _bookService.AddBook(book);

            if (author == null) return NotFound("Author not found.");

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {

            var Out = await _bookService.DeleteBook(id);

            if (!Out) return NotFound();

            return NoContent();
        }
    }
}
