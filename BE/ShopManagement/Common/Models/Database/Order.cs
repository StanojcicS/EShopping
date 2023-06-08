using ShopManagement.Common.Models.DataBase;
using System.Text.Json.Serialization;

namespace ShopManagement.Common.Models.Database
{
    public class Order
    {
        public string UserId { get; set; }
        public Guid Id { get; set; } 
        public DateTime DeliveringTime { get; set; }
        public double TotalPrice { get; set; }
        public bool OrderDeclinedd { get; set; }
        public string? Comment { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime OrderedAt { get; set; }


        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public List<OrderProduct> OrderProducts { get; set; } = new();

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public List<Product> Products { get; set; } = new();
    }
}
