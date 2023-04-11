using Microsoft.EntityFrameworkCore;
using UserManagement.Models.DataBase;

namespace UserManagement.Data
{
    public class UserDataContext : DbContext
    {
        public UserDataContext(DbContextOptions<UserDataContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; } = default!;
    }
}
