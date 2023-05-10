namespace UserManagement.Common.Models.Outbound
{
    public class SellerView
    { 
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Adress { get; set; }
        public int? Verified { get; set; }
        public string? ProfilePictureUrl { get; set; } 
    }
}
