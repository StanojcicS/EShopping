using UserManagement.Data;
using UserManagement.Repositories.Contracts;

namespace UserManagement.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserDataContext _context;
        private bool _disposed;

        public IUserRepository Users { get; }

        public UnitOfWork(UserDataContext context,
                          IUserRepository users)
        {
            _context = context;
            Users = users;
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
