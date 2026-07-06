using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderManagement.API.DTOs;
using OrderManagement.API.Model;
using OrderManagement.API.Services;

namespace OrderManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
   public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public readonly  ILogger<UserController> _logger;
        public UserController(UserService userservice, ILogger<UserController> logger) 
        {
         _userService = userservice;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterDTO user)
        {
            var res = await _userService.RegisterUser(user);
            if (res.Success) 
            {
                _logger.LogInformation("User Register Sucessfully");
                return Ok(res);
            }
            _logger.LogError("User Register Failed");
            return BadRequest(res);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginDTO user)
        {
            var res = await _userService.LoginUser(user);
            if (res.Success)
            {
                _logger.LogInformation("User logined Sucessfully");
                return Ok(res);
            }
            _logger.LogError("User logined Failed");
            return BadRequest(res);
        }

        [HttpGet("user/:id")]
        public async Task<IActionResult> GetUser(int id)
        {
            var res = await _userService.GetUser(id);
            if (res.Success)
            {
                _logger.LogInformation("Sucessfully fetched the user");
                return Ok(res);
            }
            _logger.LogError("Failed To Get the user");
            return BadRequest(res);
        }
    }
}
