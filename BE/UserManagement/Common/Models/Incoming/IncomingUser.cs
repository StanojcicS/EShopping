using UserManagement.Common.Enumerations;

namespace UserManagement.Common.Models.Incoming
{
    public class IncomingUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Adress { get; set; }
        public UserType UserType { get; set; }
    }
}
