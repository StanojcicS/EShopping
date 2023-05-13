using Microsoft.AspNetCore.Mvc;
using ShopManagement.Common.Models.DataBase;
using ShopManagement.Common.Models.Inbound;

namespace ShopManagement.Services.Contracts
{
    public interface IProductService
    {
        Task<Product> AddNewProduct(NewProduct newProduct);
    }
}
