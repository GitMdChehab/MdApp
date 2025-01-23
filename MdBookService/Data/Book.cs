using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static MdBookService.Data.BookCategory;

namespace MdBookService.Data
{
    public class Book
    {
        [Key]
        public long Id { get; set; }
        public string BookName { get; set; }
        public int Quantity { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public Author Author { get; set; }
        public BookCategory Category { get; set; }
    }
}