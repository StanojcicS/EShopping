using UserManagement.Models.DataBase;
using UserManagement.Models.Incoming;

namespace UserManagement.Services.Contracts
{
    public interface IUserService
    {
        Task<User> CreateUser(IncomingUser user);
        Task<User> UpdateUser(User user);
        Task<User> DeleteUser(User user);
        Task<User> GetUserById(Guid id);
        Task<User> GetAll();

    }
}
