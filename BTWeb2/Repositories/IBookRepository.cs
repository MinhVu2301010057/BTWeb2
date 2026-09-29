using BTWeb2.Models.Domain;
using BTWeb2.Models.DTO;

namespace BTWeb2.Repositories
{
    public interface IBookRepository
    {
        List<BookWithAuthorAndPublisherDTO> GetAllBooks();
        BookWithAuthorAndPublisherDTO? GetBookById(int id);
        AddBookRequestDTO AddBook(AddBookRequestDTO addBookRequestDTO);
        AddBookRequestDTO? UpdateBookById(int id, AddBookRequestDTO bookDTO);
        Books? DeleteBookById(int id);
    }
}