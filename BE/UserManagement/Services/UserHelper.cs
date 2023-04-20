using UserManagement.Services.Contracts;

namespace UserManagement.Services
{
    public class UserHelper : IUserHelper
    {
        public async Task<byte[]> ParseProfilePictureToBytes(IFormFile incomingPicture)
        {
            byte[] bytePicture;
            if (incomingPicture != null && incomingPicture.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await incomingPicture.CopyToAsync(memoryStream);
                    bytePicture = memoryStream.ToArray();
                }
            }
            else
                    bytePicture = new byte[0];
            return bytePicture;
        }
    }
}
