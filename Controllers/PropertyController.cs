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

        var property = new Property
        {
            PropertyType = request.PropertyType,
            Location = request.Location,
            Pincode = request.Pincode,
            Price = request.Price,
            Description = request.Description,
            Amenities = request.Amenities,
            Status = request.Status,
            AddedBy = request.UserId,
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
    public async Task<IActionResult> UpdatePropertyPartial(int id, [FromBody] PropertyPatchDto request)
    {
        // Fetch the existing property from your data source
        var existingProperty = await _context.Properties
            .Include(p => p.PropertyImages) // Include images if needed
            .FirstOrDefaultAsync(p => p.PropertyId == id);

        if (existingProperty == null)
        {
            return NotFound($"Property with ID {id} not found.");
        }

        // Update fields only if they have values in the request
        if (request.PropertyType.HasValue)
        {
            existingProperty.PropertyType = request.PropertyType.Value;
        }

        if (!string.IsNullOrWhiteSpace(request.Location))
        {
            existingProperty.Location = request.Location;
        }

        if (!string.IsNullOrWhiteSpace(request.Pincode))
        {
            existingProperty.Pincode = request.Pincode;
        }

        if (request.Price.HasValue)
        {
            existingProperty.Price = request.Price.Value;
        }

        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            existingProperty.Description = request.Description;
        }

        if (!string.IsNullOrWhiteSpace(request.Amenities))
        {
            existingProperty.Amenities = request.Amenities;
        }

        if (request.Status.HasValue)
        {
            existingProperty.Status = request.Status.Value;
        }

        // Handle ImageFiles if provided in the request
        if (request.ImageFiles != null && request.ImageFiles.Any())
        {
            foreach (var file in request.ImageFiles)
            {
                if (file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(_storagePath, fileName);

                    // Ensure the directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    existingProperty.PropertyImages.Add(new PropertyImage { FilePath = filePath });
                }
            }
        }
        await _context.SaveChangesAsync();

        return NoContent(); // Return a response indicating success
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
        await _context.SaveChangesAsync();
        return Ok(property);
    }
}