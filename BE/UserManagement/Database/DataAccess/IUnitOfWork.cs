using UserManagement.Database.Repositories.Contracts; 

namespace UserManagement.Database.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }

        Task<int> SaveChanges();
    }
}
