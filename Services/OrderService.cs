using OrderManagement.API.Model;
using OrderManagement.API.Repositories;
using OrderManagement.API.Utills;
using OrderManagement.API.DTOs;

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
                var products = await _productRepository.GetProductsFromDB(userId,-1,-1);
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
                    TotalPrice = price * quantity,
                    Status = "Placed",
                    OrderDate = DateTime.UtcNow,
                    Items = new[] 
                    { 
                        new OrderItemModel 
                        { 
                            ProductId = productId, 
                            Quantity = quantity, 
                            Price = price 
                        } 
                    }
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

        public async Task<ApiResponse<List<OrderDTO>>> GetOrders(int userId)
        {
            try
            {
                var orders = await _orderRepository.GetOrdersByUser(userId);
                return new ApiResponse<List<OrderDTO>> { Success = true, Data = orders, Message = "Orders retrieved successfully" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<OrderDTO>> { Success = false, Message = "Failed to retrieve orders: " + ex.Message };
            }
        }

        public async Task<ApiResponse<List<OrderModel>>> CheckoutCart(int userId)
        {
            try
            {
                var cartItems = await _cartRepository.GetCartsFromDB(userId);
                if (cartItems == null || !cartItems.Any())
                {
                    return new ApiResponse<List<OrderModel>> { Success = false, Message = "Cart is empty" };
                }

                decimal grandTotal = 0;
                var orderItems = new List<OrderItemModel>();

                foreach (var item in cartItems)
                {
                    if (!decimal.TryParse(item.Price, out decimal price))
                    {
                        price = 0;
                    }
                    
                    orderItems.Add(new OrderItemModel
                    {
                        ProductId = item.Id,
                        Quantity = item.Quantity,
                        Price = price
                    });
                    
                    grandTotal += price * item.Quantity;
                }

                var order = new OrderModel
                {
                    UserId = userId,
                    TotalPrice = grandTotal,
                    Status = "Placed",
                    OrderDate = DateTime.UtcNow,
                    Items = orderItems.ToArray()
                };

                var createdOrder = await _orderRepository.CreateOrder(order);

                // Clear cart completely after successful order
                var user = await _cartRepository.CancelCartFromDB(new CartModel { ProductId = cartItems.First().Id, Quantity = 0 }, userId);
                if (user != null) {
                    user.Cart = Array.Empty<CartModel>();
                }
                // It is better to create a ClearCart in CartRepo, but we can do it here by injecting DbContext if needed, 
                // wait, the easiest way is to use existing cartItems to remove all:
                foreach(var item in cartItems) 
                {
                    await _cartRepository.RemoveCartFromDB(new CartModel { ProductId = item.Id, Quantity = item.Quantity }, userId);
                }

                return new ApiResponse<List<OrderModel>> { Success = true, Data = new List<OrderModel> { createdOrder }, Message = "Checkout successful" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<OrderModel>> { Success = false, Message = "Failed to checkout: " + ex.Message };
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
