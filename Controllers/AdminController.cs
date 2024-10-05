using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PropertySales.Data;
using PropertySales.Models.Domain;
using Microsoft.EntityFrameworkCore; // Import this for DbContext extensions
using System.Collections.Generic; // Import this for List<T>
using System.Threading.Tasks; // Import this for Task
using Microsoft.Extensions.Configuration;

namespace PropSalesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly PropertySalesDbContext _context;
        private readonly string _imageBasePath;

        public AdminController(PropertySalesDbContext context ,IConfiguration configuration)
        {
            _context = context;
            _imageBasePath = configuration["ImageStorage:Path"];
        }

        [HttpGet("GetBrokers")]
        public async Task<IActionResult> getBrokers()
        {
            List<Broker> brokers = await _context.Brokers.ToListAsync(); // Use async method
            if (brokers == null || brokers.Count == 0)
            {
                return NotFound("No brokers found.");
            }
            return Ok(brokers);
        }

        [HttpGet("GetCustomers")]
        public async Task<IActionResult> getCustomers()
        {
            List<User> users = await _context.Users.ToListAsync(); // Use async method
            if (users == null || users.Count == 0)
            {
                return NotFound("No users found."); // Corrected message
            }
            return Ok(users);
        }

        [HttpGet("GetProperties")]
        public async Task<IActionResult> GetProperties()
        {
            var properties = await _context.Properties
                .Include(p => p.PropertyImages) // Include PropertyImages
                .ToListAsync();

            if (properties?.Count == 0) // Simplified null check
            {
                return NotFound("No properties found.");
            }

            // Update the file paths to be web-accessible URLs
            foreach (var property in properties)
            {
                foreach (var image in property.PropertyImages)
                {
                    // Replace the local file path with a web-accessible one
                    image.FilePath = image.FilePath
                        .Replace(Path.Combine(Directory.GetCurrentDirectory(), _imageBasePath) + "\\", "/Uploads/");
                }
            }

            return Ok(properties);
        }


        [HttpGet("GetTransactions")]
        public async Task<IActionResult> getTransactions()
        {
            List<Transaction> transactions = await _context.Transactions.ToListAsync(); // Use async method
            if (transactions == null || transactions.Count == 0)
            {
                return NotFound("No transactions found."); // Corrected message
            }
            return Ok(transactions);
        }


        [HttpDelete("Property{id}")]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            var property = await _context.Properties.Include(p => p.PropertyImages).FirstOrDefaultAsync(p => p.PropertyId == id);

            if (property == null)
            {
                return NotFound($"No Entry with id: {id}");
            }
            _context.Properties.Remove(property);
            return Ok(property);
        }

        [HttpDelete("User{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.UserId == id);

            if (user == null)
            {
                return NotFound($"No Entry with id: {id}");
            }
            _context.Users.Remove(user);
            return Ok(user);
        }

        [HttpDelete("Broker{id}")]
        public async Task<IActionResult> DeleteBroker(int id)
        {
            var broker = await _context.Brokers.FirstOrDefaultAsync(p => p.BrokerId == id);

            if (broker == null)
            {
                return NotFound($"No Entry with id: {id}");
            }
            _context.Brokers.Remove(broker);
            return Ok(broker);
        }
    }
}
