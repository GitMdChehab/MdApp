using MdBorrowService.Data;
using MdBorrowService.Interfaces;
using MdBorrowService.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Serialization;

namespace MdBorrowService.Services
{
    public class BorrowService(BorrowContext db, IConfiguration config) : IBorrowService
    {
        // Existing methods (e.g., GetAllBorrowsAsync, CreateBorrowAsync, etc.)
        public async Task<IEnumerable<BookDTO>> AvailableBooksAsync(string token)
        {
            var books = await GetAllBooksAsync(token);
            if (books != null)
            {
                var borrows = await db.Borrows.Where(p => p.Status == BorrowStatus.Overdue).GroupBy(g => g.BookId)
                      .Select(s => new { bookId = s.Key, NbBorrowed = s.Count() }).ToListAsync();
                return books.Where(p => !borrows.Any(p2 => p2.bookId == p.Id && p2.NbBorrowed >= p.Quantity)).AsEnumerable();
            }
            return books;
        }
        public async Task<List<Borrow>> BorrowsAsync(long bookId, int userId, string token)
        {
            var borrows = await db.Borrows.Where(p => p.UserId == userId).ToListAsync();
            return borrows;
        }
        public async Task<bool> ReserveBookAsync(long bookId, int userId, string token)
        {
            var book = await GetBookByIdAsync(bookId, token);

            if (book == null || book.Quantity <= 0)
                return false; // Book not available for reservation

            // Create a borrow record with "Pending" status
            var borrow = new Borrow
            {
                BookId = bookId,
                UserId = userId,
                BorrowDate = DateTime.Now,
                Status = BorrowStatus.Pending
            };

            db.Borrows.Add(borrow);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> BorrowBookAsync(long bookId, int userId, string token)
        {
            //var book = await db.Books.FindAsync(bookId);

            //if (book == null || book.Quantity <= 0)
            //    return false; // Book not available for borrowing

            // Deduct book quantity
            //book.Quantity--;

            // Create a borrow record
            var borrow = new Borrow
            {
                BookId = bookId,
                UserId = userId,
                BorrowDate = DateTime.Now,
                Status = BorrowStatus.Pending
            };

            db.Borrows.Add(borrow);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReturnBookAsync(long borrowId, string token)
        {
            var borrow = await db.Borrows.FindAsync(borrowId);
            if (borrow == null || borrow.Status == BorrowStatus.Returned)
                return false; // Borrow record not found or already returned

            // Mark as returned
            borrow.Status = BorrowStatus.Returned;
            borrow.ReturnDate = DateTime.Now;

            // Increment book quantity
            //var book = await db.Books.FindAsync(borrow.BookId);
            //if (book != null)
            //{
            //    book.Quantity++;
            //}

            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateBorrowStatusAsync(long borrowId, BorrowStatus status)
        {
            var borrow = await db.Borrows.FindAsync(borrowId);
            if (borrow == null)
                return false;

            borrow.Status = status;
            await db.SaveChangesAsync();
            return true;
        }
        private async Task<BookDTO?> GetBookByIdAsync(long id, string token)
        {
            return await ApiService.GetAsync<BookDTO>(config["ServicesUrl:BookById"]!, token);
        }
        private async Task<IEnumerable<BookDTO>?> GetAllBooksAsync(string token)
        {
            return await ApiService.GetAsync<IEnumerable<BookDTO>>(config["ServicesUrl:GetAllBooks"]!, token);
        }
    }

}
