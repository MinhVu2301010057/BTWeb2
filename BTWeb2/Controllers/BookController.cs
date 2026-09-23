using BTWeb2.Data;
using BTWeb2.Models.Domain;
using BTWeb2.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTWeb2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public BookController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        [Route("get-book-by-id/{id:int}")]
        public IActionResult GetBookById([FromRoute] int id)
        {
            var bookDomain = _dbContext.Books
                .Include(b => b.Publisher)
                .Include(b => b.Book_Authors).ThenInclude(ba => ba.Author)
                .FirstOrDefault(b => b.Id == id);

            if (bookDomain == null)
            {
                return NotFound();
            }

            var bookDTO = new BookDTO()
            {
                Id = bookDomain.Id,
                Title = bookDomain.Title,
                Description = bookDomain.Description,
                IsRead = bookDomain.IsRead,
                DateRead = bookDomain.DateRead,
                Rate = bookDomain.Rate,
                Genre = bookDomain.Genre,
                CoverUrl = bookDomain.CoverUrl,
                DateAdded = bookDomain.DateAdded,
                PublisherName = bookDomain.Publisher != null ? bookDomain.Publisher.Name : "Unknown",
                AuthorNames = bookDomain.Book_Authors?.Where(y => y.Author != null).Select(y => y.Author.FullName).ToList() ?? new List<string>()
            };
            return Ok(bookDTO);
        }
            [HttpPost("add-book")]
            public IActionResult AddBook([FromBody] AddBookRequestDTO addBookRequestDTO)
            {
                var publisherDomain = _dbContext.Publishers.FirstOrDefault(x => x.Id == addBookRequestDTO.PublisherID);
                if (publisherDomain == null)
                {
                    return NotFound(new { message = "Không tìm thấy NXB" });
                }
                var bookDomain = new Books()
                {
                    Title = addBookRequestDTO.Title,
                    Description = addBookRequestDTO.Description,
                    IsRead = addBookRequestDTO.IsRead,
                    DateRead = addBookRequestDTO.DateRead,
                    Rate = addBookRequestDTO.Rate,
                    Genre = addBookRequestDTO.Genre,
                    CoverUrl = addBookRequestDTO.CoverUrl,
                    DateAdded = (DateTime)addBookRequestDTO.DateAdded,
                    PublisherID = publisherDomain.Id
                };
                _dbContext.Books.Add(bookDomain);
                _dbContext.SaveChanges();
                if (addBookRequestDTO.AuthorIds != null)
                {
                    foreach (var authorId in addBookRequestDTO.AuthorIds)
                    {
                        var authorDomain = _dbContext.Authors.FirstOrDefault(x => x.Id == authorId);
                        if (authorDomain == null)
                        {
                            return NotFound(new { message = "Không tìm thấy tác giả" });
                        }

                        var bookAuthorDomain = new Book_Author()
                        {
                            BookId = bookDomain.Id,
                            AuthorId = authorDomain.Id
                        };

                        _dbContext.Book_Authors.Add(bookAuthorDomain);
                        _dbContext.SaveChanges();
                    }
                }

                return Ok();
            }
        [HttpPut("update-book-by-id/{id:int}")]
        public IActionResult UpdateBookById(int id, [FromBody] AddBookRequestDTO addBookRequestDTO)
        {
            // 1. Cập nhật thông tin cơ bản của Book
            var bookDomain = _dbContext.Books.FirstOrDefault(x => x.Id == id);
            if (bookDomain != null)
            {
                bookDomain.Title = addBookRequestDTO.Title;
                bookDomain.Description = addBookRequestDTO.Description;
                bookDomain.IsRead = addBookRequestDTO.IsRead;
                bookDomain.DateRead = addBookRequestDTO.DateRead;
                bookDomain.Rate = addBookRequestDTO.Rate;
                bookDomain.Genre = addBookRequestDTO.Genre;
                bookDomain.CoverUrl = addBookRequestDTO.CoverUrl;
                bookDomain.DateAdded = (DateTime)addBookRequestDTO.DateAdded;
                bookDomain.PublisherID = addBookRequestDTO.PublisherID;

                _dbContext.SaveChanges();
            }
            else
            {
                return NotFound();
            }

            // 2. Xóa các tác giả cũ của sách này trong bảng trung gian Book_Authors
            var existingBookAuthors = _dbContext.Book_Authors.Where(x => x.BookId == id).ToList();
            if (existingBookAuthors != null && existingBookAuthors.Count > 0)
            {
                _dbContext.Book_Authors.RemoveRange(existingBookAuthors);
                _dbContext.SaveChanges();
            }

            // 3. Thêm lại danh sách tác giả mới được gửi lên
            if (addBookRequestDTO.AuthorIds != null)
            {
                foreach (var authorId in addBookRequestDTO.AuthorIds)
                {
                    var authorDomain = _dbContext.Authors.FirstOrDefault(x => x.Id == authorId);
                    if (authorDomain == null)
                    {
                        return NotFound();
                    }

                    var bookAuthorDomain = new Book_Author()
                    {
                        BookId = bookDomain.Id,
                        AuthorId = authorDomain.Id
                    };

                    _dbContext.Book_Authors.Add(bookAuthorDomain);
                    _dbContext.SaveChanges();
                }
            }

            return Ok(addBookRequestDTO);
        }
    }
}

