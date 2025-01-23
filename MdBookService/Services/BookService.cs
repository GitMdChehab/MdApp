using MdBookService.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using MdBookService.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MdBookService.Models;

namespace MdBookService.Services
{
    public class BookService(BookContext db) : IBookService
    {
        public async Task<IEnumerable<BookDTO>> GetAllBooksAsync(FilterBookRB request)
        {
            var query = db.Books
                .Include(b => b.Author)
                .Include(b => b.Category);
            if (!string.IsNullOrEmpty(request.AuthorName)) query = (IIncludableQueryable<Book, BookCategory>)query.Where(p => p.Author.AuthorName.Contains(request.AuthorName.Trim()));
            if (!string.IsNullOrEmpty(request.BookName)) query = (IIncludableQueryable<Book, BookCategory>)query.Where(p => p.Author.AuthorName.Contains(request.BookName.Trim()));
            if (!string.IsNullOrEmpty(request.CategoryName)) query = (IIncludableQueryable<Book, BookCategory>)query.Where(p => p.Author.AuthorName.Contains(request.CategoryName.Trim()));

            return await query.Select(b => new BookDTO
            {
                Id = b.Id,
                BookName = b.BookName,
                Quantity = b.Quantity,
                AuthorName = b.Author.AuthorName,
                CategoryName = b.Category.CategoryName
            })
            .ToListAsync();
        }
        public async Task<BookDTO> GetBookByIdAsync(long id)
        {
            var book = await db.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return null;

            return new BookDTO
            {
                Id = book.Id,
                BookName = book.BookName,
                Quantity = book.Quantity,
                AuthorId = book.AuthorId,
                CategoryId = book.CategoryId,
                AuthorName = book.Author.AuthorName,
                CategoryName = book.Category.CategoryName
            };
        }
        public async Task<BookDTO> CreateBookAsync(Book book)
        {
            db.Books.Add(book);
            await db.SaveChangesAsync();
            return await GetBookByIdAsync(book.Id);
        }
        public async Task<BookDTO> UpdateBookAsync(Book book)
        {
            var dbBook = await db.Books.FindAsync(book.Id);
            if (dbBook == null) return null;

            dbBook.BookName = book.BookName;
            dbBook.Quantity = book.Quantity;
            dbBook.AuthorId = book.AuthorId;
            dbBook.CategoryId = book.CategoryId;

            db.Books.Update(dbBook);
            await db.SaveChangesAsync();
            return await GetBookByIdAsync(dbBook.Id);
        }
        public async Task<bool> DeleteBookAsync(long id)
        {
            var book = await db.Books.FindAsync(id);
            if (book == null) return false;

            db.Books.Remove(book);
            await db.SaveChangesAsync();

            return true;
        }
    }

}