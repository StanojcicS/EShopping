using AutoMapper;
using BCrypt.Net;
using Common.Exceptions.CustomExceptions;
using System.Security.Claims;
using UserManagement.Common.Models.DataBase;
using UserManagement.Common.Models.Incoming;
using UserManagement.Database.DataAccess;
using UserManagement.Services.Contracts;

namespace UserManagement.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJwtHelper _jwtHelper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IJwtHelper jwtHelper)
        {
            _jwtHelper = jwtHelper;
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

        public async Task<string> Login(string username, string password)
        {
            var user = await _unitOfWork.Users.FindAsync(u => u.UserName.Equals(username));
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Role, user.UserType.ToString()));
                claims.Add(new Claim("UserId", user.UserId.ToString()));
                return _jwtHelper.GetNewJwtToken(claims, user.UserId.ToString());
            }
            throw new InvalidCredentialsException("There is no user with this username and password.");
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



        public async Task<User> UpdateUser(User user)
        {
            var userToBeUpdated = await _unitOfWork.Users.FindAsync(u => u.UserName == user.UserName);

            User updatedUser = new User()
            {
                UserId = userToBeUpdated.UserId,
                UserName = userToBeUpdated.UserName,
                Adress = userToBeUpdated.Adress,
                DateOfBirth = userToBeUpdated.DateOfBirth,
                Password = userToBeUpdated.Password,
                Name = userToBeUpdated.Name,
                LastName = userToBeUpdated.LastName,
                UserType = userToBeUpdated.UserType,
                ProfilePicture = userToBeUpdated.ProfilePicture,
                Verified = userToBeUpdated.Verified,
                Email = userToBeUpdated.Email
            };

            await _unitOfWork.Users.Update(updatedUser, updatedUser.UserId);
            await _unitOfWork.SaveChanges();

            return updatedUser;
        }

        public async Task<string> Verify(string username)
        {
            var existingUser = await _unitOfWork.Users.FindAsync(u => u.UserName.Equals(username));
            if(existingUser == null)
                throw new InvalidUsernameException("There is no user with the given username.");
            existingUser.Verified = 1;
            var verifiedUser = UpdateUser(existingUser);
            return verifiedUser.Result.UserName;
        }
    }
}
