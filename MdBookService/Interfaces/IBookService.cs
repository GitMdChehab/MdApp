using MdBookService.Data;
using MdBookService.Models;

namespace MdBookService.Interfaces
{
    public interface IBookService
    {
        Task<BookDTO> CreateBookAsync(Book book);
        Task<bool> DeleteBookAsync(long id);
        Task<IEnumerable<BookDTO>> GetAllBooksAsync(FilterBookRB request);
        Task<BookDTO> GetBookByIdAsync(long id);
        Task<BookDTO> UpdateBookAsync(Book book);
    }
}