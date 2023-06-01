using Microsoft.AspNetCore.Mvc;
using ShopManagement.Common.Models.DataBase;
using ShopManagement.Common.Models.Inbound;
using ShopManagement.Common.Models.Outbound;

namespace ShopManagement.Services.Contracts
{
    public interface IProductService
    {
        Task<Product> AddNewProduct(NewProduct newProduct);
        Task<List<Product>> GetAll();
        Task<List<ProductView>> GetAllBySellerId(string sellerId);
        Task<Product> Update(UpdateProduct product);
        Task<string> Delete(DeleteProduct product);
    }
}
