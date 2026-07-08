using OrderManagement.API.DataContext;
using OrderManagement.API.Model;
using OrderManagement.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace OrderManagement.API.Repositories
{
    public class OrderRepository
    {
        private readonly AppDBContext _context;

        public OrderRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<OrderModel> CreateOrder(OrderModel order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<List<OrderDTO>> GetOrdersByUser(int userId)
        {
            var orders = await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
            var products = await _context.Products.ToListAsync();

            return orders.Select(o => {
                var orderItems = o.Items.Select(item => {
                    var p = products.FirstOrDefault(prod => prod.Id == item.ProductId);
                    return new OrderItemDTO
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        ProductName = p?.ProductName ?? "Unknown",
                        Description = p?.Description ?? "",
                        Category = p?.Category ?? "",
                        ProductImage = p?.ProductImage ?? ""
                    };
                }).ToArray();

                return new OrderDTO
                {
                    Id = o.Id,
                    UserId = o.UserId,
                    TotalPrice = o.TotalPrice,
                    Status = o.Status,
                    OrderDate = o.OrderDate,
                    Items = orderItems
                };
            }).ToList();
        }

        public async Task<OrderModel?> GetOrderById(int orderId)
        {
            return await _context.Orders.FindAsync(orderId);
        }

        public async Task<OrderModel?> CancelOrder(int orderId, int userId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o=>o.Id==orderId);
            if (order != null)
            {
                order.Status = "Cancelled";
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            return order;
        }
    }
}
