using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PropertySales.Data;
using PropertySales.Models.Domain;
using Microsoft.EntityFrameworkCore; // Import this for DbContext extensions
using System.Collections.Generic; // Import this for List<T>
using System.Threading.Tasks; // Import this for Task

namespace PropSalesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly PropertySalesDbContext _context;

        public AdminController(PropertySalesDbContext context)
        {
            _context = context;
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
        public async Task<IActionResult> getProperties()
        {
            List<Property> properties = await _context.Properties.ToListAsync(); // Use async method
            if (properties == null || properties.Count == 0)
            {
                return NotFound("No properties found."); // Corrected message
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
    }
}
