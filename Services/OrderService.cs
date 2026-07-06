using OrderManagement.API.Model;
using OrderManagement.API.Repositories;
using OrderManagement.API.Utills;

namespace OrderManagement.API.Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly ProductRepository _productRepository;
        private readonly CartRepository _cartRepository;

        public OrderService(OrderRepository orderRepository, ProductRepository productRepository, CartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
        }

        public async Task<ApiResponse<OrderModel>> CreateOrder(int userId, string productId, int quantity)
        {
            try
            {
                // Fetch product to calculate total price
                var products = await _productRepository.GetProductsFromDB(userId);
                var product = products.FirstOrDefault(p => p.Id == productId);

                if (product == null)
                    return new ApiResponse<OrderModel> { Success = false, Message = "Product not found" };

                if (!decimal.TryParse(product.Price, out decimal price))
                {
                    price = 0;
                }

                var order = new OrderModel
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    TotalPrice = price * quantity,
                    Status = "Placed",
                    OrderDate = DateTime.UtcNow
                };

                var createdOrder = await _orderRepository.CreateOrder(order);
                await _cartRepository.RemoveCartFromDB(new CartModel { ProductId = productId, Quantity = quantity }, userId);
                return new ApiResponse<OrderModel> { Success = true, Data = createdOrder, Message = "Order created successfully" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<OrderModel> { Success = false, Message = "Failed to create order: " + ex.Message };
            }
        }

        public async Task<ApiResponse<List<OrderModel>>> GetOrders(int userId)
        {
            try
            {
                var orders = await _orderRepository.GetOrdersByUser(userId);
                return new ApiResponse<List<OrderModel>> { Success = true, Data = orders, Message = "Orders retrieved successfully" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<OrderModel>> { Success = false, Message = "Failed to retrieve orders: " + ex.Message };
            }
        }

        public async Task<ApiResponse<OrderModel>> CancelOrder(int orderId, int userId)
        {
            try
            {
                var cancelledOrder = await _orderRepository.CancelOrder(orderId, userId);
                if (cancelledOrder == null)
                {
                    return new ApiResponse<OrderModel> { Success = false, Message = "Order not found or already cancelled" };
                }
                return new ApiResponse<OrderModel> { Success = true, Data = cancelledOrder, Message = "Order cancelled successfully" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<OrderModel> { Success = false, Message = "Failed to cancel order: " + ex.Message };
            }
        }
    }
}
