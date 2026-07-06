using System.Data.Entity;
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
        public async Task<List<ProductModel>> GetProductsFromDB(int id)
        {
           var res = _context.Products.ToList();
            return res;
        }
        public async Task<ProductModel> GetProductFromDB(int id)
        {
            var res = await _context.Products.FindAsync(id);
            return res;
        }
       
    }
}
