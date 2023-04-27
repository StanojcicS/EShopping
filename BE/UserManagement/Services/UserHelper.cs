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

        public async Task<string> ParseProfilePictureToString(byte[] picture)
        {
            if (picture == null)
                return null;
            return $"data:image/jpg;base64,{Convert.ToBase64String(picture)}"; 
        }
    }
}
