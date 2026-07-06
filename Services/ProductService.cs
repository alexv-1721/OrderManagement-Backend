using OrderManagement.API.Model;
using OrderManagement.API.Repositories;
using OrderManagement.API.Utills;

namespace OrderManagement.API.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;
        public ProductService(ProductRepository productRepository) 
        {
        _productRepository = productRepository;
        }
        public async Task<ApiResponse<List<ProductModel>>> GetProducts(int userid)
        {
            try
            {
                var res = await _productRepository.GetProductsFromDB(userid);
                return new ApiResponse<List<ProductModel>> { Data = res, Success = true, Message = "Success" };
            }
            catch
            {
                return new ApiResponse<List<ProductModel>> { Success = false, Message = "Database Error" };
            }
        }

        public async Task<ApiResponse<List<ProductModel>>> GetProduct(int proId)
        {
            try
            {
                var res = await _productRepository.GetProductsFromDB(proId);
                return new ApiResponse<List<ProductModel>> { Data = res, Success = true, Message = "Success" };
            }
            catch
            {
                return new ApiResponse<List<ProductModel>> { Success = false, Message = "Database Error" };
            }
        }
    }
}
