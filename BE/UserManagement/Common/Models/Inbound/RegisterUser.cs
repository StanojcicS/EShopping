using UserManagement.Common.Enumerations;

namespace UserManagement.Common.Models.Inbound
{
    public class RegisterUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Adress { get; set; }
        public string UserType { get; set; }
        public IFormFile? ProfilePicture { get; set; }
    }
}
