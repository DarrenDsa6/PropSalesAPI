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

    private readonly PropertySalesDbContext _context; // Replace with your actual DbContext
    private readonly string _storagePath;

    public PropertyController(PropertySalesDbContext context, IConfiguration configuration)
    {
        _context = context;
        // Combine the project directory with the Uploads folder
        var uploadsFolder = configuration["ImageStorage:Path"];
        _storagePath = Path.Combine(Directory.GetCurrentDirectory(), uploadsFolder);

        // Ensure the directory exists
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
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(_storagePath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                property.PropertyImages.Add(new PropertyImage { FilePath = filePath });
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
            .Include(p => p.PropertyImages) // Include images if needed
            .ToListAsync();

        return Ok(properties);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> EditProperty(int id, [FromBody] PropertyUploadRequest request)
    {
        var property = await _context.Properties.Include(p => p.PropertyImages).FirstOrDefaultAsync(p => p.PropertyId == id);

        if (property == null)
        {
            return NotFound($"Property with ID {id} not found.");
        }

        if (request.PropertyType != null)
        {
            property.PropertyType = request.PropertyType;
        }

        if (!string.IsNullOrEmpty(request.Location))
        {
            property.Location = request.Location;
        }

        if (!string.IsNullOrEmpty(request.Pincode))
        {
            property.Pincode = request.Pincode;
        }

        if (request.Price > 0)
        {
            property.Price = request.Price;
        }

        if (!string.IsNullOrEmpty(request.Description))
        {
            property.Description = request.Description;
        }

        if (!string.IsNullOrEmpty(request.Amenities))
        {
            property.Amenities = request.Amenities;
        }

        if (request.Status != null)
        {
            property.Status = request.Status;
        }


        await _context.SaveChangesAsync();

        return Ok(property);
    }


    [HttpDelete("{id}")]
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
}