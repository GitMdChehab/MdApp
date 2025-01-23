namespace MdBookService.Models
{
    public class BookDTO : FilterBookRB
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
    }
}