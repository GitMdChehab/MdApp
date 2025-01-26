namespace MdBookService.Models
{
    public class FilterBookRB
    {
        public List<long> BookIds { get; set; }
        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public string CategoryName { get; set; }
    }
}