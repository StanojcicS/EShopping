using ShopManagement.Common.Models.Database;
using ShopManagement.Common.Models.Inbound;
using ShopManagement.Common.Models.Outbound;

namespace ShopManagement.Services.Contracts
{
    public interface IOrderService
    {
        Task<Order> CreateNewOrder(NewOrder order);
        Task<List<OrderView>> GetAllByBuyerId(string userId);
        Task<bool> CancelOrder(CancelOrder order);
        Task<OrderDetailsView> OrderDetails(OrderDetailsInbound orderDetailsInbound);
        Task<List<OrderView>> GetNewOrdersForSeller(string userId);
    }
}
