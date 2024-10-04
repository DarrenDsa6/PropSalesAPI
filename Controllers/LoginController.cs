using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertySales.Data;
using PropertySales.Models.ViewModels;
using System.Threading.Tasks; // Import this for Task

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
        public async Task<IActionResult> LoginUser([FromBody] UserLoginValidation request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == request.UserName); // Use async method

            if (user == null)
                return Unauthorized("Invalid username or password.");

            if (user.Password != request.Password) // Use hashed password comparison
                return Unauthorized("Invalid username or password.");

            return Ok(new { userId = user.UserId, userName = user.UserName });
        }

        [HttpPost("LoginBroker")]
        public async Task<IActionResult> LoginBroker([FromBody] UserLoginValidation request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if the broker exists in the database
            var user = await _context.Brokers
                .FirstOrDefaultAsync(u => u.UserName == request.UserName); // Use async method

            if (user == null)
                return Unauthorized("Invalid username or password.");

            // Replace this with a proper password hashing check
            if (user.Password != request.Password) // Use hashed password comparison
                return Unauthorized("Invalid username or password.");

            return Ok(new { userId = user.BrokerId, userName = user.UserName });
        }
    }
}
