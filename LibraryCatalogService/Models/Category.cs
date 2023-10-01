using Common.Model;

namespace LibraryCatalogService.Models
{
    public class Category : TimeStampBase
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}
