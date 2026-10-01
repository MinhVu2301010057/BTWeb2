using BTWeb2.Data;
using BTWeb2.Models.Domain;
using BTWeb2.Models.DTO;
using BTWeb2.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BTWeb2.CustomActionFilter;

namespace BTWeb2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet("get-all-books")]
        public IActionResult GetAll()
        {
            var allBooks = _bookRepository.GetAllBooks();
            return Ok(allBooks);
        }

        [HttpGet("get-book-by-id/{id:int}")]
        public IActionResult GetBookById([FromRoute] int id)
        {
            var bookDTO = _bookRepository.GetBookById(id);
            if (bookDTO == null)
            {
                return NotFound();
            }
            return Ok(bookDTO);
        }

        [HttpPost("add-book")]
        [ValidateModel]
        public IActionResult AddBook([FromBody] AddBookRequestDTO addBookRequestDTO)
        {
            if (ModelState.IsValid)
            {
                var bookAdded = _bookRepository.AddBook(addBookRequestDTO);
                return Ok(bookAdded);
            }
            else return BadRequest(ModelState);
        }

        [HttpPut("update-book-by-id/{id:int}")]
        public IActionResult UpdateBookById(int id, [FromBody] AddBookRequestDTO addBookRequestDTO)
        {
            var updatedBook = _bookRepository.UpdateBookById(id, addBookRequestDTO);
            if (updatedBook == null)
            {
                return NotFound();
            }
            return Ok(updatedBook);
        }

        [HttpDelete("delete-book-by-id/{id:int}")]
        public IActionResult DeleteBookById(int id)
        {
            var deletedBook = _bookRepository.DeleteBookById(id);
            if (deletedBook == null)
            {
                return NotFound();
            }
            return Ok(deletedBook);
        }
    }
}

