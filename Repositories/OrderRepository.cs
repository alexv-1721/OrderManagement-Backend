using OrderManagement.API.DataContext;
using OrderManagement.API.Model;
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

        public async Task<List<OrderModel>> GetOrdersByUser(int userId)
        {
            return await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
        }

        public async Task<OrderModel?> GetOrderById(int orderId)
        {
            return await _context.Orders.FindAsync(orderId);
        }

        public async Task<OrderModel?> CancelOrder(int orderId, int userId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
            if (order != null)
            {
                order.Status = "Cancelled";
                await _context.SaveChangesAsync();
            }
            return order;
        }
    }
}
