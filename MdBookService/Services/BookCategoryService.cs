
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MdBookService.Data;

namespace MdBookService.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MdBookService.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class BookCategoryService(BookContext db) : IBookCategoryService
    {
        public async Task<IEnumerable<BookCategory>> GetAllAsync()
        {
            return await db.BookCategories.ToListAsync();
        }
        public async Task<BookCategory> GetByIdAsync(int id)
        {
            return await db.BookCategories.FindAsync(id);
        }
        public async Task<BookCategory> AddAsync(BookCategory category)
        {
            db.BookCategories.Add(category);
            await db.SaveChangesAsync();
            return category;
        }
        public async Task<BookCategory> UpdateAsync(BookCategory category)
        {
            db.BookCategories.Update(category);
            await db.SaveChangesAsync();
            return category;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var category = await db.BookCategories.FindAsync(id);
            if (category == null) return false;

            db.BookCategories.Remove(category);
            await db.SaveChangesAsync();
            return true;
        }
    }

}