using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertySales.Models.Domain
{
    public class Property
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PropertyId { get; set; }

        [Required]
        [Column(TypeName = "varchar(10)")]
        public PropertyType PropertyType { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Pincode { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public string Description { get; set; }

        public string Amenities { get; set; }

        public virtual ICollection<PropertyImage> PropertyImages { get; set; } = new List<PropertyImage>();

        [Required]
        [Column(TypeName = "varchar(10)")]
        public PropertyStatus Status { get; set; }

        [ForeignKey("User")]
        public int AddedBy { get; set; }
    }

    public enum PropertyType
    {
        Sale,
        Rent
    }

    public enum PropertyStatus
    {
        Active,
        Pending,
        Sold,
        Rented
    }
}
