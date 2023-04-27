using UserManagement.Common.Models.DataBase;
using UserManagement.Common.Models.Inbound;
using UserManagement.Common.Models.Outbound;

namespace UserManagement.Services.Contracts
{
    public interface IUserService
    {
        Task<User> CreateUser(RegisterUser user);
        Task<User> UpdateUser(UpdateUser user);
        Task<User> DeleteUser(User user);
        Task<User> GetUserById(Guid id);
        Task<User> GetAll();
        Task<LogedInUser> Login(string username, string password);
        Task<string> Verify(string username);

    }
}
