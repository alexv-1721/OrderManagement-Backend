using OrderManagement.API.DataContext;
using OrderManagement.API.Model;
using Microsoft.EntityFrameworkCore;

namespace OrderManagement.API.Repositories
{
    public class CartRepository
    {
        private readonly AppDBContext _context;

        public CartRepository(AppDBContext Context)
        {
            _context = Context;
        }

        public async Task<List<ProductModel>> GetCartsFromDB(int uid)
        {
            var user = await _context.Users.FindAsync(uid);
            if (user == null || user.Cart == null) 
                return new List<ProductModel>();
            var products = await _context.Products.ToListAsync();
            return products.Where(p => user.Cart.Any(c => c.ProductId == p.Id)).ToList();
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
                await _context.SaveChangesAsync();
            }
            return user;
        }


    }
}
