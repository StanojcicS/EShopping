namespace ShopManagement.Common.Models.Outbound
{
    public class OrderDetailsView
    {
        public string OrderedAt { get; set; }
        public string DeliveringTime { get; set; }
        public double TotalPrice { get; set; }
        public string? Comment { get; set; }
        public string Address { get; set; }
        public bool IsCanceled { get; set; }
        public List<ProductView> Products { get; set; }
    }
}
