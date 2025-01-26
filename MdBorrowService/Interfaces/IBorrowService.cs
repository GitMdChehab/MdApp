using MdBorrowService.Data;
using MdBorrowService.Models;

namespace MdBorrowService.Interfaces
{
    public interface IBorrowService
    {
        Task<IEnumerable<BookDTO>> AvailableBooksAsync(string token);
        Task<bool> BorrowBookAsync(long bookId, int userId, string token);
        Task<List<Borrow>> BorrowsAsync( int userId, string token);
        Task<bool> ReserveBookAsync(long bookId, int userId, string token);
        Task<bool> ReturnBookAsync(long borrowId, string token);
    }
}