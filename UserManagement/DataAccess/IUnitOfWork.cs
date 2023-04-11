using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Repositories.Contracts;

namespace UserManagement.DataAccess
{
    public interface IUnitOfWork : IDisposable
    { 
        IUserRepository Users { get; }

        Task<int> SaveChanges();
    }
}
