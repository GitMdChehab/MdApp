using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace MdBookService.Data
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [Unique]
        public string AuthorName { get; set; }
    }
}