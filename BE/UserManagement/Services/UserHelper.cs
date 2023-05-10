using System.Text;
using UserManagement.Services.Contracts;

namespace UserManagement.Services
{
    public class UserHelper : IUserHelper
    {
        public Task<string> GetUserIdForFacebookUser(string facebookId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(facebookId.ToUpper());
            for(int i = facebookId.Length; i < 32; i++)
            {
                sb.Append('0');
            }
            sb.Insert(8, '-');
            sb.Insert(13, '-');
            sb.Insert(18, '-');
            sb.Insert(23, '-');
            return Task.FromResult(sb.ToString());
        }

        public async Task<string> GetUsernameForFacebookUser(string fullname)
        {
            string[] names = fullname.Split(' ');
            StringBuilder usernameSB = new StringBuilder();
            usernameSB.Append(names[0]);
            if (!string.IsNullOrEmpty(names[1]))
            {
                usernameSB.Append(names[1][0]);
            }
            return usernameSB.ToString();
        }

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
