using UserManagement.Common.Models.DataBase; 
using UserManagement.Database.DataAccess;
using UserManagement.Database.Repositories.Contracts;

namespace UserManagement.Database.Repositories
{
    public class UserRepository : GenericRepository<User, UserDataContext>, IUserRepository
    {
        public UserRepository(UserDataContext context) : base(context)
        {
        }
    }
}
