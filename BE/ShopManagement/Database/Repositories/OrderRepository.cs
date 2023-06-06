using ShopManagement.Common.Models.Database;
using ShopManagement.Database.DataAccess;
using ShopManagement.Database.Repositories.Contracts;

namespace ShopManagement.Database.Repositories
{
    public class OrderRepository : GenericRepository<Order, ShopManagementContext>, IOrderRepository
    {
        public OrderRepository(ShopManagementContext context) : base(context)
        {
        }
    }
}
