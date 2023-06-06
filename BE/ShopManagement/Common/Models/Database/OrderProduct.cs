using ShopManagement.Common.Models.DataBase;
using System.Text.Json.Serialization;

namespace ShopManagement.Common.Models.Database
{
    public class OrderProduct
    {  
        public Guid OrderId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public Order Order { get; set; } = null!;

        public Guid ProductId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public Product Product { get; set; } = null!;

        public int ProductQuantity { get; set; }
    }
}
