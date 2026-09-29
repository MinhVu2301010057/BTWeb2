using BTWeb2.Models.DTO;
using BTWeb2.Models.Domain;

namespace BTWeb2.Repositories
{
    public class PublisherWithBooksDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BookSimpleDTO> Books { get; set; }
    }
    public class BookSimpleDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        public int? Rate { get; set; }
        public string? Genre { get; set; }
        public string? CoverUrl { get; set; }
    }
}
