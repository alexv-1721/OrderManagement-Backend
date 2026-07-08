using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.API.Services;

namespace OrderManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest request)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(id, out int userId))
            {
                var res = await _orderService.CreateOrder(userId, request.ProductId, request.Quantity);
                if (res.Success)
                {
                    return Ok(res);
                }
                return BadRequest(res);
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(id, out int userId))
            {
                var res = await _orderService.GetOrders(userId);
                if (res.Success)
                {
                    return Ok(res);
                }
                return BadRequest(res);
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpPost("cancel/{orderId}")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(id, out int userId))
            {
                var res = await _orderService.CancelOrder(orderId, userId);
                if (res.Success)
                {
                    return Ok(res);
                }
                return BadRequest(res);
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpPost("checkout")]
        public async Task<IActionResult> CheckoutCart()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(id, out int userId))
            {
                var res = await _orderService.CheckoutCart(userId);
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
