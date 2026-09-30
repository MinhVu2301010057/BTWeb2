using BTWeb2.Models.Domain;
using BTWeb2.Models.DTO;
using BTWeb2.Repositories;

namespace BTWeb2.Repositories
{
    public interface IAuthorRepository
    {
        List<AuthorDTO> GetAllAuthors();
        AuthorNoIdDTO GetAuthorById(int id);
        AddAuthorRequestDTO AddAuthor(AddAuthorRequestDTO addAuthorRequestDTO);
        AuthorNoIdDTO? UpdateAuthorById(int id, AuthorNoIdDTO authorDTO);
        Authors? DeleteAuthorById(int id);
    }
}
