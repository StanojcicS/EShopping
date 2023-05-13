using ShopManagement.Services.Contracts;

namespace ShopManagement.Services
{
    public class ProductHelper : IProductHelper
    {
        public async Task<byte[]> ParseProductImageToBytes(IFormFile incomingImage)
        {
            byte[] byteImage;
            if (incomingImage != null && incomingImage.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await incomingImage.CopyToAsync(memoryStream);
                    byteImage = memoryStream.ToArray();
                }
            }
            else
                byteImage = new byte[0];
            return byteImage;
        }
    }
}
