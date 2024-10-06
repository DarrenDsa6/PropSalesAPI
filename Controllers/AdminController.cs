using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PropertySales.Data;
using PropertySales.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace PropSalesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly PropertySalesDbContext _context;
        private readonly string _imageBasePath;

        public AdminController(PropertySalesDbContext context, IConfiguration configuration)
        {
            _context = context;
            _imageBasePath = configuration["ImageStorage:Path"];
        }

        [HttpGet("brokers")]
        public async Task<ActionResult<List<Broker>>> GetBrokers()
        {
            var brokers = await _context.Brokers.ToListAsync();
            if (brokers == null || brokers.Count == 0)
            {
                return NotFound("No brokers found.");
            }
            return Ok(brokers);
        }

        [HttpGet("broker/{id}")]
        public async Task<ActionResult<Broker>> GetBroker(int id)
        {
            var broker = await _context.Brokers.FindAsync(id);
            if (broker == null)
            {
                return NotFound($"No Broker with id: {id}");
            }
            return Ok(broker);
        }

        [HttpGet("customers")]
        public async Task<ActionResult<List<User>>> GetCustomers()
        {
            var users = await _context.Users.ToListAsync();
            if (users == null || users.Count == 0)
            {
                return NotFound("No users found.");
            }
            return Ok(users);
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound($"No User with id: {id}");
            }
            return Ok(user);
        }

        [HttpGet("properties")]
        public async Task<ActionResult<List<Property>>> GetProperties()
        {
            var properties = await _context.Properties
                .Include(p => p.PropertyImages)
                .ToListAsync();

            if (properties?.Count == 0)
            {
                return NotFound("No properties found.");
            }

            foreach (var property in properties)
            {
                foreach (var image in property.PropertyImages)
                {
                    image.FilePath = image.FilePath
                        .Replace(Path.Combine(Directory.GetCurrentDirectory(), _imageBasePath) + "\\", "/Uploads/");
                }
            }

            return Ok(properties);
        }

        [HttpGet("transactions")]
        public async Task<ActionResult<List<Transaction>>> GetTransactions()
        {
            var transactions = await _context.Transactions.ToListAsync();
            if (transactions == null || transactions.Count == 0)
            {
                return NotFound("No transactions found.");
            }
            return Ok(transactions);
        }

        [HttpDelete("property/{id}")]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            var property = await _context.Properties
                .Include(p => p.PropertyImages)
                .FirstOrDefaultAsync(p => p.PropertyId == id);

            if (property == null)
            {
                return NotFound($"No Property with id: {id}");
            }

            _context.Properties.Remove(property);
            await _context.SaveChangesAsync(); // Persist changes
            return NoContent(); // Return 204 No Content
        }

        [HttpDelete("user/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound($"No User with id: {id}");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(); // Persist changes
            return NoContent(); // Return 204 No Content
        }

        [HttpDelete("broker/{id}")]
        public async Task<IActionResult> DeleteBroker(int id)
        {
            var broker = await _context.Brokers.FindAsync(id);
            if (broker == null)
            {
                return NotFound($"No Broker with id: {id}");
            }

            _context.Brokers.Remove(broker);
            await _context.SaveChangesAsync(); // Persist changes
            return NoContent(); // Return 204 No Content
        }
    }
}
