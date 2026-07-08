namespace OrderManagement.API.DTOs
{
    public class CartDTO
    {
        public required string Id { get; set; } = string.Empty;
        public required string ProductName { get; set; }
        public required string Description { get; set; }
        public required string Category { get; set; }
        public required string Rating { get; set; }
        public required string ProductImage { get; set; }
        public required string Price { get; set; }
        public required string ReviewCount { get; set; }
        public int Quantity { get; set; }

    }
}
