using OrderManagement.API.DTOs;
using OrderManagement.API.Model;
using OrderManagement.API.Repositories;
using OrderManagement.API.Utills;

namespace OrderManagement.API.Services
{
    public class CartService
    {
        private readonly CartRepository _cartRepository;

        public CartService(CartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<ApiResponse<List<CartDTO>>> GetCart(int userId)
        {
            try
            {
                var carts = await _cartRepository.GetCartsFromDB(userId);
                return new ApiResponse<List<CartDTO>> { Success = true, Data = carts, Message = "Cart retrieved" };
            }
            catch
            {
                return new ApiResponse<List<CartDTO>> { Success = false, Message = "Database Error" };
            }
        }

        public async Task<ApiResponse<int>> AddToCart(CartModel cart,int uid)
        {
            try
            {
                var user = await _cartRepository.AddCartToDB(cart,uid);
                if (user == null) return new ApiResponse<int> { Success = false, Message = "User not found" };
                //need to change the response
                return new ApiResponse<int> { Success = true, Data = uid, Message = "Added to cart" };
            }
            catch
            {
                return new ApiResponse<int> { Success = false, Message = "Database Error" };
            }
        }

        public async Task<ApiResponse<int>> CancelCart(CartModel cart, int userId)
        {
            try
            {
                var user = await _cartRepository.CancelCartFromDB(cart, userId);
                if (user == null) return new ApiResponse<int> { Success = false, Message = "User not found" };
                return new ApiResponse<int> { Success = true, Data = userId, Message = "Cart cancelled" };
            }
            catch
            {
                return new ApiResponse<int> { Success = false, Message = "Database Error" };
            }
        }
    }
}
