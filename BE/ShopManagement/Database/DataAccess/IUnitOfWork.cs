using ShopManagement.Database.Repositories.Contracts;

namespace ShopManagement.Database.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IOrderRepository Orders { get; }
        IOrderProductRepository OrderProducts { get; }
        Task<int> SaveChanges();
    }
}
