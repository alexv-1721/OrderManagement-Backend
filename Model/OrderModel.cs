using System.ComponentModel.DataAnnotations;

namespace OrderManagement.API.Model
{
    public class OrderModel
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Placed"; // Placed, Cancelled
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    }
}
