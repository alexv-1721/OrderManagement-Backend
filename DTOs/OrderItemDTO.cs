namespace OrderManagement.API.DTOs
{
    public class OrderItemDTO
    {
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        
        // Product Details
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string ProductImage { get; set; } = string.Empty;
    }
}
