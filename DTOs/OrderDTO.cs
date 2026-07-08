namespace OrderManagement.API.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public OrderItemDTO[] Items { get; set; } = Array.Empty<OrderItemDTO>();
    }
}
public class OrderRequest
{
    public string ProductId { get; set; } = string.Empty;
    public int Quantity { get; set; }
}