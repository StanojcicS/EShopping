using ShopManagement.Database.Repositories.Contracts;

namespace ShopManagement.Database.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        Task<int> SaveChanges();
    }
}
