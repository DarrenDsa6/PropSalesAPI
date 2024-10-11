using Microsoft.AspNetCore.Http;
using PropertySales.Models.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class PropertyUploadRequest
{
    public int UserId { get; set; } // Aadhaar card number as long

    public PropertyType PropertyType { get; set; }
    public string Location { get; set; }
    public string Pincode { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string Amenities { get; set; }
    public PropertyStatus Status { get; set; }

    public List<IFormFile> ImageFiles { get; set; } = new List<IFormFile>();
}
