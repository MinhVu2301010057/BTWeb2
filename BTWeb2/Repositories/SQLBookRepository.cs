using Microsoft.EntityFrameworkCore;
using BTWeb2.Data;
using BTWeb2.Models.Domain;
using BTWeb2.Models.DTO;

namespace BTWeb2.Repositories
{
    public class SQLBookRepository : IBookRepository
    {
        private readonly AppDbContext _dbContext;

        public SQLBookRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<BookWithAuthorAndPublisherDTO> GetAllBooks()
        {
            return _dbContext.Books
                .Select(book => new BookWithAuthorAndPublisherDTO
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    IsRead = book.IsRead,
                    DateRead = book.IsRead ? book.DateRead : null,
                    Rate = book.IsRead ? book.Rate : null,
                    Genre = book.Genre,
                    CoverUrl = book.CoverUrl,
                    PublisherName = book.Publisher != null ? book.Publisher.Name : null,
                    AuthorNames = book.Book_Authors
                        .Select(ba => ba.Author.FullName)
                        .ToList()
                })
                .ToList();
        }

        public BookWithAuthorAndPublisherDTO? GetBookById(int id)
        {
            return _dbContext.Books
                .Where(n => n.Id == id)
                .Select(book => new BookWithAuthorAndPublisherDTO
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    IsRead = book.IsRead,
                    DateRead = book.DateRead,
                    Rate = book.Rate,
                    Genre = book.Genre,
                    CoverUrl = book.CoverUrl,
                    PublisherName = book.Publisher != null ? book.Publisher.Name : null,
                    AuthorNames = book.Book_Authors
                        .Select(ba => ba.Author.FullName)
                        .ToList()
                })
                .FirstOrDefault();
        }

        public AddBookRequestDTO? AddBook(AddBookRequestDTO addBookRequestDTO)
        {
            // Kiểm tra Publisher có tồn tại không
            var publisherExists = _dbContext.Publishers.Any(p => p.Id == addBookRequestDTO.PublisherID);
            if (!publisherExists)
                return null;

            var bookDomainModel = new Books
            {
                Title = addBookRequestDTO.Title,
                Description = addBookRequestDTO.Description,
                IsRead = addBookRequestDTO.IsRead,
                DateRead = addBookRequestDTO.DateRead,
                Rate = addBookRequestDTO.Rate,
                Genre = addBookRequestDTO.Genre,
                CoverUrl = addBookRequestDTO.CoverUrl,
                DateAdded = addBookRequestDTO.DateAdded ?? DateTime.Now,
                PublisherID = addBookRequestDTO.PublisherID
            };

            _dbContext.Books.Add(bookDomainModel);
            _dbContext.SaveChanges();

            // Thêm các tác giả liên kết
            if (addBookRequestDTO.AuthorIds != null && addBookRequestDTO.AuthorIds.Any())
            {
                foreach (var authorId in addBookRequestDTO.AuthorIds)
                {
                    _dbContext.Book_Authors.Add(new Book_Author
                    {
                        BookId = bookDomainModel.Id,
                        AuthorId = authorId
                    });
                }
                _dbContext.SaveChanges();
            }

            return addBookRequestDTO;
        }

        public AddBookRequestDTO? UpdateBookById(int id, AddBookRequestDTO bookDTO)
        {
            var bookDomain = _dbContext.Books.FirstOrDefault(n => n.Id == id);
            if (bookDomain == null)
                return null;

            // Kiểm tra Publisher mới có tồn tại không
            var publisherExists = _dbContext.Publishers.Any(p => p.Id == bookDTO.PublisherID);
            if (!publisherExists)
                return null;

            bookDomain.Title = bookDTO.Title;
            bookDomain.Description = bookDTO.Description;
            bookDomain.IsRead = bookDTO.IsRead;
            bookDomain.DateRead = bookDTO.DateRead;
            bookDomain.Rate = bookDTO.Rate;
            bookDomain.Genre = bookDTO.Genre;
            bookDomain.CoverUrl = bookDTO.CoverUrl;
            bookDomain.DateAdded = bookDTO.DateAdded ?? DateTime.Now;
            bookDomain.PublisherID = bookDTO.PublisherID;

            // Xóa quan hệ tác giả cũ
            var oldAuthors = _dbContext.Book_Authors
                .Where(a => a.BookId == id)
                .ToList();
            _dbContext.Book_Authors.RemoveRange(oldAuthors);

            // Thêm quan hệ tác giả mới
            if (bookDTO.AuthorIds != null && bookDTO.AuthorIds.Any())
            {
                foreach (var authorId in bookDTO.AuthorIds)
                {
                    _dbContext.Book_Authors.Add(new Book_Author
                    {
                        BookId = id,
                        AuthorId = authorId
                    });
                }
            }

            // Chỉ cần SaveChanges 1 lần duy nhất
            _dbContext.SaveChanges();
            return bookDTO;
        }

        public Books? DeleteBookById(int id)
        {
            var bookDomain = _dbContext.Books.FirstOrDefault(n => n.Id == id);
            if (bookDomain == null)
                return null;

            // Xóa liên kết trong bảng trung gian trước
            var bookAuthors = _dbContext.Book_Authors
                .Where(ba => ba.BookId == id)
                .ToList();
            _dbContext.Book_Authors.RemoveRange(bookAuthors);

            // Xóa sách
            _dbContext.Books.Remove(bookDomain);
            _dbContext.SaveChanges();

            return bookDomain;
        }
    }
}