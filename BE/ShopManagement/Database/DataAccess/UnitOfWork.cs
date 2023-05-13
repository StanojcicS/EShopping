using ShopManagement.Database.Repositories.Contracts;

namespace ShopManagement.Database.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShopManagementContext _context;
        private bool _disposed;

        public IProductRepository Products { get; }
        
        public UnitOfWork(ShopManagementContext context, IProductRepository products)
        {
            _context= context;
            Products= products;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
