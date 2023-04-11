using AutoMapper;
using Common.Exceptions;
using UserManagement.DataAccess;
using UserManagement.Models.DataBase;
using UserManagement.Models.Incoming;
using UserManagement.Services.Contracts;

namespace UserManagement.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<User> CreateUser(IncomingUser incomingUser)
        {
            var user = _mapper.Map<User>(incomingUser);
            user.UserId = Guid.NewGuid();
            user.Password = BCrypt.Net.BCrypt.HashPassword(incomingUser.Password);

            var existingId = await _unitOfWork.Users.GetById(user.UserId);
            var existingUsername = await _unitOfWork.Users.FindAsync(u => u.UserName.Equals(user.UserName));
            if (existingId == null && existingUsername == null)
            {
                _unitOfWork.Users.Insert(user);
                _unitOfWork.SaveChanges();
                return user;
            }
            else
            {
                throw new UserAlreadyExistsException("User with this username(or Id) already exists.");
            }
            
        }

        public Task<User> DeleteUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
