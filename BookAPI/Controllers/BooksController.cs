using BookAPI.Models;
using BookAPI.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

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
        public IActionResult AddBook([FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _bookRepository.AddBook(book);
                return CreatedAtAction(nameof(GetBookById), new { id = book.BookID }, book);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            try
            {
                var books = _bookRepository.GetBooks();
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("search")]
        public IActionResult GetBooks([FromQuery] string? genre, [FromQuery] string? author, [FromQuery] string? sortColumn, [FromQuery] string? sortOrder)
        {
            try
            {
                var books = _bookRepository.GetBooks(genre, author, sortColumn, sortOrder);
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            try
            {
                var book = _bookRepository.GetBooks().FirstOrDefault(b => b.BookID == id);
                if (book == null)
                {
                    return NotFound();
                }
                return Ok(book);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingBook = _bookRepository.GetBooks().FirstOrDefault(b => b.BookID == id);
                if (existingBook == null)
                {
                    return NotFound();
                }

                book.BookID = id;
                _bookRepository.UpdateBook(book);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult SoftDeleteBook(int id)
        {
            try
            {
                var existingBook = _bookRepository.GetBooks().FirstOrDefault(b => b.BookID == id);
                if (existingBook == null)
                {
                    return NotFound();
                }

                _bookRepository.SoftDeleteBook(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
