using System.Data.Entity;
using System.Drawing.Printing;
using OrderManagement.API.DataContext;
using OrderManagement.API.Model;

namespace OrderManagement.API.Repositories
{
    public class ProductRepository
    {
        private readonly AppDBContext _context;
        public ProductRepository(AppDBContext Context) 
        {
            _context = Context;
        }
        public Task<List<ProductModel>> GetProductsFromDB(int id, int page,int pageSize)
        {
            if (id > 0) 
            { 
                var products = _context.Products.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
                return Task.FromResult(products);
            }

           var res = _context.Products.ToList();
            return Task.FromResult(res);
        }
        public async Task<ProductModel?> GetProductFromDB(string id)
        {
            var res = await _context.Products.FindAsync(id);
            return res;
        }
       
    }
   }
