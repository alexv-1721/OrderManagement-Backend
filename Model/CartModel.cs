using System.ComponentModel.DataAnnotations;

namespace OrderManagement.API.Model
{
    public class CartModel
    {
        [Key]
        public int Id { get; set; }
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
