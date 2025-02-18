using Microsoft.AspNetCore.Mvc;
using Library.Models;
using Microsoft.AspNetCore.Cors;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {

        private readonly AuthorService _authorService;

        public AuthorsController(AuthorService authorService)
        {
            _authorService = authorService;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            return await _authorService.GetAllAuthors();
        }


        [HttpGet("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var Out = await _authorService.GetAuthorById(id);

            if (Out == null) return NotFound();

            return Out;
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor(Author author)
        {
            await _authorService.AddAuthor(author);

            return CreatedAtAction("GetAuthor", new { id = author.Id }, author);
        }
    }
}