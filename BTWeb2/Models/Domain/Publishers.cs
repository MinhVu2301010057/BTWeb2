using System.ComponentModel.DataAnnotations;

namespace BTWeb2.Models.Domain
{
    public class Publishers
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Books> Books { get; set; }
    }
}
