using MdBookService.Data;

namespace MdBookService.Interfaces
{
    public interface IBookCategoryService
    {
        Task<BookCategory> AddAsync(BookCategory category);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<BookCategory>> GetAllAsync();
        Task<BookCategory> GetByIdAsync(int id);
        Task<BookCategory> UpdateAsync(BookCategory category);
    }
}