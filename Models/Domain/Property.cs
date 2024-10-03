using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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

        public string Images { get; set; } // Store file paths or URLs

        [Required]
        [Column(TypeName = "varchar(10)")]
        public PropertyStatus Status { get; set; }

        [ForeignKey("User")]
        public int AddedBy { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>(); // Initialize
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
