using Common.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryCatalogService.Models
{
    public class Book : TimeStampBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
