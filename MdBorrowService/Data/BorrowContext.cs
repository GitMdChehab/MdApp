using Microsoft.EntityFrameworkCore;

namespace MdBorrowService.Data
{
    public class BorrowContext : DbContext
    {
        public BorrowContext(DbContextOptions<BorrowContext> options) : base(options)
        {
        }
        public DbSet<Borrow> Borrows { get; set; }
    }
}
