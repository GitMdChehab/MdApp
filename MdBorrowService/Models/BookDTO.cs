namespace MdBorrowService.Models
{
    public class BookDTO 
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public string CategoryName { get; set; }
    }
}