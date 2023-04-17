using UserManagement.Common.Models.DataBase;
using UserManagement.Common.Models.Incoming;

namespace UserManagement.Services.Contracts
{
    public interface IUserService
    {
        Task<User> CreateUser(IncomingUser user);
        Task<User> UpdateUser(User user);
        Task<User> DeleteUser(User user);
        Task<User> GetUserById(Guid id);
        Task<User> GetAll();
        Task<string> Login(string username, string password);
        Task<string> Verify(string username);

    }
}
