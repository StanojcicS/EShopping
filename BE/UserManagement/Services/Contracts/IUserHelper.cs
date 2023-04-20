namespace UserManagement.Services.Contracts
{
    public interface IUserHelper
    {
        Task<byte[]> ParseProfilePictureToBytes(IFormFile picture);
    }
}
