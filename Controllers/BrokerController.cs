using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertySales.Data;
using PropertySales.Models.Domain;
using PropSalesAPI.Models.ViewModels;

namespace PropSalesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrokerController : ControllerBase
    {

        private readonly PropertySalesDbContext _context;

        public BrokerController(PropertySalesDbContext context)
        {
            _context = context;
        }

        [HttpPost("AddBuyer/{id}")]
        public async Task<IActionResult> AddBuyer(int id, [FromBody] AddBuyerDto request)
        {
            var user = await _context.Users
            .FirstOrDefaultAsync(u => u.AadhaarCard == request.AadhaarCard);

            if (user == null)
            {
                var broker = new Broker
                {
                    Name = request.Name,
                    UserName = request.UserName,
                    Password = request.Password, // Consider hashing passwords
                    ContactNumber = request.ContactNumber,
                    Address = request.Address,
                    Pincode = request.Pincode,
                    AdhaarCard = request.AadhaarCard
                };

                await _context.Brokers.AddAsync(broker); // Use async method
                await _context.SaveChangesAsync();

            }
            var userId = await _context.Users
            .Where(u => u.AadhaarCard == request.AadhaarCard)
            .Select(u => u.UserId)
            .FirstOrDefaultAsync();

            var property = await _context.Properties
            .Include(p => p.PropertyImages) // Include images if needed
            .FirstOrDefaultAsync(p => p.PropertyId == id);

            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.PropertyId == id);

            if (transaction == null)
            {
                return NotFound($"Property with ID {id} not found in tr.");
            }
            transaction.BuyerId = userId;
            return Ok(transaction);
        }


    }
}
