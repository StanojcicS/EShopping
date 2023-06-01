namespace ShopManagement.Common.Models.Outbound
{
    public class ProductView
    {
        public Guid Id { get; set; }
        public Guid SellerId { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    
    }
}
