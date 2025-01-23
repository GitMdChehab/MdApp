using MdBookService.Data;
using MdBookService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MdBookService.Services
{

    namespace MdBookAuthorService.Services
    {
        public class AuthorService(BookContext db) : IAuthorService
        {
            public async Task<IEnumerable<Author>> GetAllAsync()
            {
                return await db.Authors.ToListAsync();
            }

            public async Task<Author> GetByIdAsync(int id)
            {
                return await db.Authors.FindAsync(id);
            }

            public async Task<Author> AddAsync(Author author)
            {
                db.Authors.Add(author);
                await db.SaveChangesAsync();
                return author;
            }

            public async Task<Author> UpdateAsync(Author author)
            {
                db.Authors.Update(author);
                await db.SaveChangesAsync();
                return author;
            }

            public async Task<bool> DeleteAsync(int id)
            {
                var author = await db.Authors.FindAsync(id);
                if (author == null) return false;

                db.Authors.Remove(author);
                await db.SaveChangesAsync();
                return true;
            }
        }
    }

}