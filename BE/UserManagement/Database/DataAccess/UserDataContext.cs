using Microsoft.EntityFrameworkCore;
using UserManagement.Common.Models.DataBase;

namespace UserManagement.Database.DataAccess
{
    public class UserDataContext : DbContext
    {
        public UserDataContext(DbContextOptions<UserDataContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; } = default!;
    }
}
