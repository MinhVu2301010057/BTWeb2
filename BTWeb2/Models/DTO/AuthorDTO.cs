using BTWeb2.Models.Domain;
using BTWeb2.Models.DTO;
using BTWeb2.Repositories;

namespace BTWeb2.Models.DTO
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
    }
    public class AuthorNoIdDTO
    {
        public string FullName { get; set; }
    }
}
