using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using OrderManagement.API.DataContext;
using OrderManagement.API.Model;


[Authorize]
public class ProductController : ODataController
{
    private readonly AppDBContext _context;
    
    public ProductController(AppDBContext context)
    {
        _context = context;
    }

    [EnableQuery]
    public IQueryable<ProductModel> Get()
    {
        return _context.Products;
    }
}