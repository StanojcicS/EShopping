using ShopManagement.Common.Models.Database;
using ShopManagement.Common.Models.Inbound;

namespace ShopManagement.Services.Contracts
{
    public interface IOrderService
    {
        Task<Order> CreateNewOrder(NewOrder order);
    }
}
