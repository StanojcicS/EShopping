using ShopManagement.Common.Models.Database;
using ShopManagement.Database.DataAccess;
using ShopManagement.Database.Repositories.Contracts;

namespace ShopManagement.Database.Repositories
{
    public class OrderProductRepository : GenericRepository<OrderProduct, ShopManagementContext>, IOrderProductRepository
    {
        public OrderProductRepository(ShopManagementContext context) : base(context)
        {
        }
    }
}
