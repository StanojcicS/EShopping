using AutoMapper;
using BCrypt.Net;
using Common.Exceptions.CustomExceptions;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using UserManagement.Common.Enumerations;
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
        private readonly IConfiguration _configuration;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IJwtHelper jwtHelper, IUserHelper userHelper, IConfiguration configuration)
        {
            _jwtHelper = jwtHelper;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userHelper = userHelper;
            _configuration = configuration;
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
                await _unitOfWork.Users.Insert(user);
                await _unitOfWork.SaveChanges();
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

        public async Task<LogedInUser> FacebookLogin(FacebookLoginUser facebookUser)
        {
            var userId = await _userHelper.GetUserIdForFacebookUser(facebookUser.Id);

            var guid = new Guid(userId);

            var existingUser = await _unitOfWork.Users.FindAsync(u => u.UserId.Equals(new Guid(userId)));
            if (existingUser != null)
            {
                var userr = await GetExistingFacebookUser(existingUser); // DELETE AFTER CHECK 
                return await GetExistingFacebookUser(existingUser);
            }
            else
            {
                var userr = await RegisterNewFacebookUser(facebookUser);
                return userr;
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

        public async Task<LogedInUser> UpdateUser(UpdateUser user)
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

            var updatedUser = _mapper.Map<LogedInUser>(userUpdatedValues);
            if (userCurrentValues.UserId.ToString().Contains("0000"))
                updatedUser.ProfilePicture = Encoding.ASCII.GetString(userUpdatedValues.ProfilePicture);
            else
                updatedUser.ProfilePicture = await _userHelper.ParseProfilePictureToString(userUpdatedValues.ProfilePicture);

            return updatedUser;
        }

        public async Task<string> Verify(VerifyUserModel user)
        {
            var existingUser = await _unitOfWork.Users.FindAsync(u => u.UserName.Equals(user.Username));
            if(existingUser == null)
                throw new InvalidUsernameException("There is no user with the given username.");
            existingUser.Verified = user.Value;

            await _unitOfWork.Users.Update(existingUser, existingUser.UserId);
            await _unitOfWork.SaveChanges();

            SendVerificationEmail(existingUser.Email);

            return existingUser.UserName;
        }

        public async Task<List<SellerView>> GetSellers()
        {
            var sellersInDatabase = _unitOfWork.Users.GetAll().Result.Where(u => u.UserType == UserType.Seller).OrderBy(u => u.Verified) .ToList();
            var sellersAdapted = _mapper.Map<List<SellerView>>(sellersInDatabase);
            List<SellerView> sellersSorted = new List<SellerView>();
            sellersSorted.AddRange(sellersAdapted);
            foreach (var seller in sellersSorted)
            {
                if (seller.Verified == null)        //stavljanje odbijenih zahteva za verifikaciju na kraj liste
                {
                    var tempSeller = seller;
                    sellersAdapted.Remove(seller);
                    sellersAdapted.Add(tempSeller);
                }
            }
            return sellersAdapted;
        }
        private async Task<LogedInUser> GetExistingFacebookUser(User existingUser)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, existingUser.UserType.ToString()));
            claims.Add(new Claim("UserId", existingUser.UserId.ToString()));
            var token = _jwtHelper.GetNewJwtToken(claims, existingUser.UserId.ToString());
            var logedInUser = _mapper.Map<LogedInUser>(existingUser);
            logedInUser.ProfilePicture = existingUser.ProfilePicture == null ? null : Encoding.ASCII.GetString(existingUser.ProfilePicture);
            logedInUser.Token = token;
            return logedInUser;
        }

        private async Task<LogedInUser> RegisterNewFacebookUser(FacebookLoginUser facebookLoginUser)
        {
            var username = await _userHelper.GetUsernameForFacebookUser(facebookLoginUser.Fullname);

            while( (await _unitOfWork.Users.FindAsync(u => u.UserName.Equals(username))) != null) 
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(username);
                sb.Append('1');
                username = sb.ToString();
            }

            var userId = await _userHelper.GetUserIdForFacebookUser(facebookLoginUser.Id);
            var randomPassword = BCrypt.Net.BCrypt.HashPassword(RandomString(12));
            var name = facebookLoginUser.Fullname.Split(' ')[0];
            var lastname = string.IsNullOrEmpty(facebookLoginUser.Fullname.Split(' ')[1]) ? "unknown last name" : facebookLoginUser.Fullname.Split(' ')[1];
            var dateOfBirth = new DateTime();
            var adress = "unknown adress";
            var userType = "Buyer";
            var profilePicture = facebookLoginUser.PictureUrl == null ? null : Encoding.ASCII.GetBytes(facebookLoginUser.PictureUrl);

            var user = new User()
            {
                UserId = new Guid(userId),
                UserName = username,
                Email = facebookLoginUser.Email,
                Password = randomPassword,
                Name = name,
                LastName = lastname,
                DateOfBirth = dateOfBirth,
                Adress = adress,
                UserType = (UserType)Enum.Parse(typeof(UserType), userType),
                ProfilePicture = profilePicture,
                Verified = 0
            };

            _unitOfWork.Users.Insert(user);
            _unitOfWork.SaveChanges();

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, user.UserType.ToString()));
            claims.Add(new Claim("UserId", user.UserId.ToString()));
            var token = _jwtHelper.GetNewJwtToken(claims, user.UserId.ToString());
            var logedInUser = _mapper.Map<LogedInUser>(user);
            logedInUser.ProfilePicture = user.ProfilePicture == null ? null : Encoding.ASCII.GetString(user.ProfilePicture);
            logedInUser.Token = token;
            return logedInUser;
        }


        private static Random random = new Random();

        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void SendVerificationEmail(string email)
        { 
            var senderEmail = _configuration.GetValue<string>("MailServiceCredentials:Email");
            var password = _configuration.GetValue<string>("MailServiceCredentials:Password");
            var stmpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderEmail, password),
                EnableSsl = true
            };

            stmpClient.Send(senderEmail, email, "Verifcation", "Admin has reviewed your verification request, log in to check it.");
        }
    }
}
