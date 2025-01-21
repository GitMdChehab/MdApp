using Microsoft.EntityFrameworkCore;

namespace MdAuthService.Data
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {
        }
        public DbSet<AppUser> Users { get; set; }
    }
}