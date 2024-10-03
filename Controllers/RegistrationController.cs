using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PropertySales.Data;
using PropertySales.Models.Domain;
using PropertySalesAPI.Models.ViewModels;

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
        public IActionResult RegisterBroker([FromBody] RegisterUserRequest request)
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

            _context.Brokers.Add(broker);
            _context.SaveChanges();
            return Ok(broker);
        }

        [HttpPost("User")]
        public IActionResult RegisterUser([FromBody] RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                Name = request.Name,
                UserName = request.UserName,
                Password = request.Password, 
                ContactNumber = request.ContactNumber,
                Address = request.Address,
                Pincode = request.Pincode,
                AdhaarCard = request.AdhaarCard
            };

            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok(user);
        }
    }
}
