using ShopManagement.Common.Models.DataBase;
using ShopManagement.Database.DataAccess;
using ShopManagement.Database.Repositories.Contracts;

namespace ShopManagement.Database.Repositories
{
    public class ProductRepository : GenericRepository<Product, ShopManagementContext>, IProductRepository
    {
        public ProductRepository(ShopManagementContext context) : base(context)
        {

        }
    }
}
