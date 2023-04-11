using UserManagement.Data;
using UserManagement.Models.DataBase;
using UserManagement.Repositories.Contracts;

namespace UserManagement.Repositories
{
    public class UserRepository : GenericRepository<User, UserDataContext>, IUserRepository
    {
        public UserRepository(UserDataContext context) : base(context)
        { 
        }
    }
}
