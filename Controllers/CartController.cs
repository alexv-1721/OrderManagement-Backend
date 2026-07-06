using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.API.Model;
using OrderManagement.API.Services;

namespace OrderManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCarts()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(id, out int userId))
            {
                var res = await _cartService.GetCart(userId);
                if (res.Success)
                {
                    return Ok(res);
                }
                return BadRequest(res);
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddCarts(CartModel cart)
        {
            var uid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(uid, out int userId))
            {
                var res = await _cartService.AddToCart(cart, userId);
                if (res.Success)
                {
                    return Ok(res);
                }
                return BadRequest(res);
            }
            return Unauthorized();
        }
    }
}
