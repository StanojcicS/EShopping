using Microsoft.EntityFrameworkCore;
using ShopManagement.Common.Models.DataBase;

namespace ShopManagement.Database.DataAccess
{
    public class ShopManagementContext : DbContext
    {
        public ShopManagementContext(DbContextOptions<ShopManagementContext> options) : base(options) { }
        public virtual DbSet<Product> Products { get; set; } = default!;
    }
}
