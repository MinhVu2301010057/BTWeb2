using BTWeb2.Models.Domain;
using BTWeb2.Models.DTO;
using BTWeb2.Repositories;

namespace BTWeb2.Repositories
{
    public interface IBookRepository
    {
        List<BookWithAuthorAndPublisherDTO> GetAllBooks(
        string? filterOn = null,
        string? filterQuery = null,
        string? sortBy = null,
        bool isAscending = true,
        int pageNumber = 1,
        int pageSize = 1000);

        BookWithAuthorAndPublisherDTO GetBookById(int id);
        AddBookRequestDTO AddBook(AddBookRequestDTO addBookRequestDTO);
        AddBookRequestDTO? UpdateBookById(int id, AddBookRequestDTO bookDTO);
        Books? DeleteBookById(int id);
    }
}