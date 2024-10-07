using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertySales.Data;
using PropertySales.Models.Domain;
using PropertySales.Models.DTO;

namespace PropSalesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly PropertySalesDbContext _context;

        public TransactionsController(PropertySalesDbContext context)
        {
            _context = context;
        }


        [HttpGet("Transactions")]
        public async Task<IActionResult> GetTransactions()
        {
            var transactions = await _context.Transactions.ToListAsync();
            if (transactions == null || transactions.Count == 0)
            {
                return NotFound("No transactions found.");
            }

            var transactionDtos = transactions.Select(t => new TransactionDto
            {
                TransactionId = t.TransactionId,
                PropertyId = t.PropertyId,
                BuyerId = t.BuyerId,
                BrokerId = t.BrokerId,
                TransactionDate = t.TransactionDate,
                Amount = t.Amount,
                Status = t.Status
            }).ToList();

            return Ok(transactionDtos);
        }


        [HttpPost("AddTransaction")]
        public async Task<IActionResult> AddTransaction([FromBody] CreateTransactionDto createTransactionDto)
        {
            if (createTransactionDto == null)
            {
                return BadRequest("Transaction data is required.");
            }

            if (createTransactionDto.Amount <= 0)
            {
                return BadRequest("Amount must be greater than zero.");
            }

            var transaction = new Transaction
            {
                PropertyId = createTransactionDto.PropertyId,
                BuyerId = createTransactionDto.BuyerId,
                BrokerId = createTransactionDto.BrokerId,
                TransactionDate = createTransactionDto.TransactionDate,
                Amount = createTransactionDto.Amount,
                Status = createTransactionDto.Status
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            var transactionDto = new TransactionDto
            {
                TransactionId = transaction.TransactionId,
                PropertyId = transaction.PropertyId,
                BuyerId = transaction.BuyerId,
                BrokerId = transaction.BrokerId,
                TransactionDate = transaction.TransactionDate,
                Amount = transaction.Amount,
                Status = transaction.Status
            };

            return CreatedAtAction(nameof(GetTransactions), new { id = transaction.TransactionId }, transactionDto);
        }
    }
}
