using UserManagement.Common.Enumerations;

namespace UserManagement.Common.Models.DataBase
{
    public class User
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Adress { get; set; }
        public UserType UserType { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public int? Verified { get; set; }
    }
}
