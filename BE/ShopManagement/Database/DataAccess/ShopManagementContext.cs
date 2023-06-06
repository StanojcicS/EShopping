using Microsoft.EntityFrameworkCore;
using ShopManagement.Common.Models.Database;
using ShopManagement.Common.Models.DataBase;

namespace ShopManagement.Database.DataAccess
{
    public class ShopManagementContext : DbContext
    {
        public ShopManagementContext(DbContextOptions<ShopManagementContext> options) : base(options) { }
        public virtual DbSet<Product> Products { get; set; } = default!;
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
