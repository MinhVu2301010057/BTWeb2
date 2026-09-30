using BTWeb2.Data;
using BTWeb2.Models.Domain;
using BTWeb2.Models.DTO;

namespace BTWeb2.Models.DTO
{
    public class PublisherDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class PublisherNoIdDTO
    {
        public string Name { get; set; }

    }
    public class PublisherWithBooksAndAuthorsDTO
    {
        public string Name { get; set; }
        public List<BookAuthorDTO> BookAuthors { set; get; }
    }
    public class BookAuthorDTO
    {
        public string BookName { get; set; }
        public List<string> BookAuthors { get; set; }
    }
}
