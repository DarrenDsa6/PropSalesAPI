using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PropertySales.Data;
using PropertySales.Models.ViewModels;

namespace PropSalesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly PropertySalesDbContext _context;

        public LoginController(PropertySalesDbContext context)
        {
            _context = context;
        }

        [HttpPost("LoginUser")]
        public IActionResult LoginUser([FromBody] UserLoginValidation request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if the user exists in the database
            var user = _context.Users.FirstOrDefault(u => u.UserName == request.UserName);

            if (user == null)
                return Unauthorized("Invalid username or password.");

            if (user.Password != request.Password) // Use hashed password comparison
                return Unauthorized("Invalid username or password.");

            return Ok(new { userId = user.UserId, userName = user.UserName });
        }

        [HttpPost("LoginBroker")]
        public IActionResult LoginBroker([FromBody] UserLoginValidation request) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if the user exists in the database
            var user = _context.Brokers.FirstOrDefault(u => u.UserName == request.UserName);

            if (user == null)
                return Unauthorized("Invalid username or password.");

            if (user.Password != request.Password) // Use hashed password comparison
                return Unauthorized("Invalid username or password.");

            return Ok(new { userId = user.BrokerId, userName = user.UserName });

        }

    }
}
