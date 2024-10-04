using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PropertySales.Data;
using PropertySales.Models.Domain;
using PropertySalesAPI.Models.ViewModels;
using System.Threading.Tasks; // Import this for Task
using Microsoft.EntityFrameworkCore; // Import this for DbContext extensions

namespace PropSalesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly PropertySalesDbContext _context;

        public RegistrationController(PropertySalesDbContext context)
        {
            _context = context;
        }

        [HttpPost("Broker")]
        public async Task<IActionResult> RegisterBroker([FromBody] RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var broker = new Broker
            {
                Name = request.Name,
                UserName = request.UserName,
                Password = request.Password, // Consider hashing passwords
                ContactNumber = request.ContactNumber,
                Address = request.Address,
                Pincode = request.Pincode,
                AdhaarCard = request.AdhaarCard
            };

            await _context.Brokers.AddAsync(broker); // Use async method
            await _context.SaveChangesAsync(); // Use async method
            return Ok(broker);
        }

        [HttpPost("User")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                Name = request.Name,
                UserName = request.UserName,
                Password = request.Password, // Consider hashing passwords
                ContactNumber = request.ContactNumber,
                Address = request.Address,
                Pincode = request.Pincode,
                AdhaarCard = request.AdhaarCard
            };

            await _context.Users.AddAsync(user); // Use async method
            await _context.SaveChangesAsync(); // Use async method
            return Ok(user);
        }
    }
}
