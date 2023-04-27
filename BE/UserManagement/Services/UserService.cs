using AutoMapper;
using BCrypt.Net;
using Common.Exceptions.CustomExceptions;
using System.Security.Claims;
using UserManagement.Common.Models.DataBase;
using UserManagement.Common.Models.Inbound;
using UserManagement.Common.Models.Outbound;
using UserManagement.Database.DataAccess;
using UserManagement.Services.Contracts;

namespace UserManagement.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJwtHelper _jwtHelper;
        private readonly IUserHelper _userHelper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IJwtHelper jwtHelper, IUserHelper userHelper)
        {
            _jwtHelper = jwtHelper;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userHelper = userHelper;
        }

        public async Task<User> CreateUser(RegisterUser incomingUser)
        {

            incomingUser.UserType = string.Concat(incomingUser.UserType[0].ToString().ToUpper(), incomingUser.UserType.Substring(1));
            var user = _mapper.Map<User>(incomingUser);
            user.UserId = Guid.NewGuid();
            user.Password = BCrypt.Net.BCrypt.HashPassword(incomingUser.Password);
            user.ProfilePicture = await _userHelper.ParseProfilePictureToBytes(incomingUser.ProfilePicture);

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

        public async Task<LogedInUser> Login(string username, string password)
        {
            var user = await _unitOfWork.Users.FindAsync(u => u.UserName.Equals(username));
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Role, user.UserType.ToString()));
                claims.Add(new Claim("UserId", user.UserId.ToString()));
                var token = _jwtHelper.GetNewJwtToken(claims, user.UserId.ToString());
                var logedInUser = _mapper.Map<LogedInUser>(user);
                logedInUser.ProfilePicture = await _userHelper.ParseProfilePictureToString(user.ProfilePicture);
                logedInUser.Token = token;
                return logedInUser;
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



        public async Task<User> UpdateUser(UpdateUser user)
        {
            var userCurrentValues = await _unitOfWork.Users.FindAsync(u => u.UserName == user.Username);
            var userUpdatedValues = _mapper.Map<User>(user);
            userUpdatedValues.UserId = userCurrentValues.UserId;
            userUpdatedValues.UserName = userCurrentValues.UserName;
            userUpdatedValues.Password = userCurrentValues.Password;
            userUpdatedValues.UserType = userCurrentValues.UserType;
            userUpdatedValues.Verified = userCurrentValues.Verified;

            if (user.ProfilePicture != null)
                userUpdatedValues.ProfilePicture = await _userHelper.ParseProfilePictureToBytes(user.ProfilePicture);
            else
                userUpdatedValues.ProfilePicture = userCurrentValues.ProfilePicture;

            await _unitOfWork.Users.Update(userUpdatedValues, userUpdatedValues.UserId);
            await _unitOfWork.SaveChanges();

            return userUpdatedValues;
        }

        public async Task<string> Verify(string username)
        {
            var existingUser = await _unitOfWork.Users.FindAsync(u => u.UserName.Equals(username));
            if(existingUser == null)
                throw new InvalidUsernameException("There is no user with the given username.");
            existingUser.Verified = 1;

            await _unitOfWork.Users.Update(existingUser, existingUser.UserId);
            await _unitOfWork.SaveChanges();

            return existingUser.UserName;
        }
    }
}
