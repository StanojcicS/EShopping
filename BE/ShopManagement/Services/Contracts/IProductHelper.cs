namespace ShopManagement.Services.Contracts
{
    public interface IProductHelper
    {
        Task<byte[]> ParseProductImageToBytes(IFormFile incomingImage);
    }
}
