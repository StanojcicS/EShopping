using ShopManagement.Common.Models.Database;
using ShopManagement.Common.Models.DataBase;

namespace ShopManagement.Services.Contracts
{
    public interface IOrderHelper
    {
        Task<double> GetTotalPrice(List<OrderProduct> products);
        Task<DateTime> GenerateDeliveringTime();
    }
}
