using ShopManagement.Common.Models.Database;
using static Common.Constants;
using ShopManagement.Services.Contracts;

namespace ShopManagement.Services
{
    public class OrderHelper : IOrderHelper
    {
        public async Task<DateTime> GenerateDeliveringTime()
        {
            Random random = new Random(); 
            return DateTime.Now.AddHours(random.Next(1, 48));
        }
         

        public async Task<double> GetTotalPrice(List<OrderProduct> products)
        {
            double totalPrice = 0;
            foreach (var product in products)
            {
                totalPrice += product.Product.Price * product.ProductQuantity;
            }
            totalPrice += SHIPPING_COSTS;
            return totalPrice;
        }
    }
}
