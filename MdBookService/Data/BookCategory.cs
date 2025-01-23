using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace MdBookService.Data
{
    public class BookCategory
    {
        [Key]
        public int Id { get; set; }
        [Unique]
        public string CategoryName { get; set; }
    }
}