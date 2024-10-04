using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PropertySales.Data;
using PropertySales.Models.Domain;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class PropertyController : ControllerBase
{
    private readonly PropertySalesDbContext _context;
    private readonly string _storagePath;
    private readonly IConfiguration _configuration; // Declare a private field for IConfiguration

    public PropertyController(PropertySalesDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration; // Assign the injected configuration to the field
        var uploadsFolder = _configuration["ImageStorage:Path"];
        _storagePath = Path.Combine(Directory.GetCurrentDirectory(), uploadsFolder);

        if (!Directory.Exists(_storagePath))
        {
            Directory.CreateDirectory(_storagePath);
        }
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddProperty([FromForm] PropertyUploadRequest request)
    {
        if (request == null)
        {
            return BadRequest("Property data is required.");
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.AadhaarCard == request.AadhaarCard);

        if (user == null)
        {
            return NotFound($"User with Aadhaar number {request.AadhaarCard} not found.");
        }

        var property = new Property
        {
            PropertyType = request.PropertyType,
            Location = request.Location,
            Pincode = request.Pincode,
            Price = request.Price,
            Description = request.Description,
            Amenities = request.Amenities,
            Status = request.Status,
            AddedBy = user.UserId,
            PropertyImages = new List<PropertyImage>()
        };

        if (request.ImageFiles == null || !request.ImageFiles.Any())
        {
            return BadRequest("At least one image file is required.");
        }

        foreach (var file in request.ImageFiles)
        {
            if (file.Length > 0)
            {
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var relativeFilePath = $"{_configuration["ImageStorage:Path"] ?? "Uploads"}/{uniqueFileName}"; // Now using the class-level field

                var filePath = Path.Combine(_storagePath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                property.PropertyImages.Add(new PropertyImage { FilePath = relativeFilePath });
            }
        }

        _context.Properties.Add(property);
        await _context.SaveChangesAsync();

        return Ok(new { property.PropertyId });
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllProperties()
    {
        var properties = await _context.Properties
            .Include(p => p.PropertyImages)
            .ToListAsync();

        return Ok(properties);
    }
}
