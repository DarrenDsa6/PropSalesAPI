using Microsoft.AspNetCore.Http;
using PropertySales.Models.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class PropertyPatchDto
{
    public PropertyType? PropertyType { get; set; } // Nullable for optional updates
    public string? Location { get; set; } // Nullable for optional updates
    public string? Pincode { get; set; } // Nullable for optional updates
    public decimal? Price { get; set; } // Nullable for optional updates
    public string? Description { get; set; } // Nullable for optional updates
    public string? Amenities { get; set; } // Nullable for optional updates
    public PropertyStatus? Status { get; set; } // Nullable for optional updates

    public List<IFormFile> ImageFiles { get; set; } = new List<IFormFile>(); // List can be empty
}
