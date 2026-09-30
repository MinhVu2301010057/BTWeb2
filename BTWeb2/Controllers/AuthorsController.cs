using BTWeb2.Data;
using BTWeb2.Models.DTO;
using BTWeb2.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BTWeb2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IAuthorRepository _authorRepository;

        public AuthorsController(AppDbContext dbContext, IAuthorRepository authorRepository)
        {
            _dbContext = dbContext;
            _authorRepository = authorRepository;
        }

        [HttpGet("get-all-authors")]
        public IActionResult GetAllAuthors()
        {
            var allAuthors = _authorRepository.GetAllAuthors();
            return Ok(allAuthors);
        }

        [HttpGet("get-author-by-id/{id:int}")]
        public IActionResult GetAuthorById([FromRoute] int id)
        {
            var authorWithId = _authorRepository.GetAuthorById(id);
            if (authorWithId == null)
            {
                return NotFound();
            }
            return Ok(authorWithId);
        }

        [HttpPost("add-author")]
        public IActionResult AddAuthors([FromBody] AddAuthorRequestDTO addAuthorRequestDTO)
        {
            var authorAdd = _authorRepository.AddAuthor(addAuthorRequestDTO);
            return Ok(authorAdd);
        }

        [HttpPut("update-author-by-id/{id:int}")]
        public IActionResult UpdateAuthorById([FromRoute] int id, [FromBody] AuthorNoIdDTO authorDTO)
        {
            var authorUpdate = _authorRepository.UpdateAuthorById(id, authorDTO);
            if (authorUpdate == null)
            {
                return NotFound();
            }
            return Ok(authorUpdate);
        }

        [HttpDelete("delete-author-by-id/{id:int}")]
        public IActionResult DeleteAuthorById([FromRoute] int id)
        {
            var authorDelete = _authorRepository.DeleteAuthorById(id);
            if (authorDelete == null)
            {
                return NotFound();
            }
            return Ok(authorDelete);
        }
    }
}