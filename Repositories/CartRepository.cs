using Microsoft.EntityFrameworkCore;
using OrderManagement.API.DataContext;
using OrderManagement.API.DTOs;
using OrderManagement.API.Model;

namespace OrderManagement.API.Repositories
{
    public class CartRepository
    {
        private readonly AppDBContext _context;

        public CartRepository(AppDBContext Context)
        {
            _context = Context;
        }
        public async Task<List<CartDTO>> GetCartsFromDB(int uid)
        {
            var user = await _context.Users.FindAsync(uid);

            if (user == null || user.Cart == null || user.Cart.Length == 0)
                return new List<CartDTO>();

            var products = await _context.Products.ToListAsync();

            var cart = products
                .Where(p => user.Cart.Any(c => c.ProductId == p.Id))
                .Select(p => new CartDTO
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Category = p.Category,
                    ProductImage = p.ProductImage,
                    Price = p.Price,
                    Rating = p.Rating,
                    ReviewCount = p.ReviewCount,
                    Quantity = user.Cart.FirstOrDefault(c => c.ProductId == p.Id)?.Quantity ?? 0
                })
                .ToList();

            return cart;
        }
        public async Task<UserModel?> AddCartToDB(CartModel cart, int uid)
        {
            var user = await _context.Users.FindAsync(uid);
            if (user != null)
            {
                var cartList = user.Cart?.ToList() ?? new List<CartModel>();
                var existingCart = cartList.FirstOrDefault(x => x.ProductId == cart.ProductId);

                if (existingCart != null)
                {
                        existingCart.Quantity += cart.Quantity;
                }
                else
                {
                    cartList.Add(cart);
                }

                user.Cart = cartList.ToArray();
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            return user;
        }

        public async Task<UserModel?> RemoveCartFromDB(CartModel cart, int uid)
        {
            var user = await _context.Users.FindAsync(uid);
            if (user != null)
            {
                var cartList = user.Cart?.ToList() ?? new List<CartModel>();
                var existingCart = cartList.FirstOrDefault(x => x.ProductId == cart.ProductId);

                if (existingCart != null)
                {
                    existingCart.Quantity -= cart.Quantity;
                    if (existingCart.Quantity <= 0)
                    {
                        cartList.Remove(existingCart);
                    }
                }

                user.Cart = cartList.ToArray();
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            return user;
        }

        public async Task<UserModel?> CancelCartFromDB(CartModel cart, int uid)
        {
            var user = await _context.Users.FindAsync(uid);
            if (user != null)
            {
                var cartList = user.Cart?.ToList() ?? new List<CartModel>();
                var existingCart = cartList.FirstOrDefault(x => x.ProductId == cart.ProductId);

                if (existingCart != null)
                {
                    cartList.Remove(existingCart);
                }

                user.Cart = cartList.ToArray();
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            return user;
        }

    }
}
