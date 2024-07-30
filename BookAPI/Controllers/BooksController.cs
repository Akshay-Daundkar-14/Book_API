using BookAPI.Models;
using BookAPI.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpPost]
        public IActionResult AddBook([FromBody]Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _bookRepository.AddBook(book);
            return CreatedAtAction(nameof(GetBookById), new { id = book.BookID }, book);
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            var books = _bookRepository.GetBooks();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            var book = _bookRepository.GetBooks().FirstOrDefault(b => b.BookID == id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingBook = _bookRepository.GetBooks().FirstOrDefault(b => b.BookID == id);
            if (existingBook == null)
            {
                return NotFound();
            }

            book.BookID = id;
            _bookRepository.UpdateBook(book);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult SoftDeleteBook(int id)
        {
            var existingBook = _bookRepository.GetBooks().FirstOrDefault(b => b.BookID == id);
            if (existingBook == null)
            {
                return NotFound();
            }

            _bookRepository.SoftDeleteBook(id);
            return NoContent();
        }
    }
}
