using BTWeb2.Models.Domain;
using BTWeb2.Models.DTO;
using BTWeb2.Repositories;

namespace BTWeb2.Models.DTO
{
    public class AuthorWithBooksDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public List<BookSimpleDTO> Books { get; set; }
    }
}
