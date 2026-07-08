using System.Drawing.Printing;
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
        public async Task<ApiResponse<List<ProductModel>>> GetProducts(int userid,int page, int pageSize)
        {
            try
            {
                var res = await _productRepository.GetProductsFromDB(userid, page, pageSize);
                return new ApiResponse<List<ProductModel>> { Data = res, Success = true, Message = "Success" };
            }
            catch
            {
                return new ApiResponse<List<ProductModel>> { Success = false, Message = "Database Error" };
            }
        }

        public async Task<ApiResponse<ProductModel>> GetProduct(string proId)
        {
            try
            {
                var res = await _productRepository.GetProductFromDB(proId);
                if (res == null) return new ApiResponse<ProductModel> { Success = false, Message = "Product not found" };
                return new ApiResponse<ProductModel> { Data = res, Success = true, Message = "Success" };
            }
            catch
            {
                return new ApiResponse<ProductModel> { Success = false, Message = "Database Error" };
            }
        }
    }
}
