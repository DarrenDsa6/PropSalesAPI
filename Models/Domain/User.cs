using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PropertySales.Models.Domain
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required] 
        public string UserName { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public long ContactNumber { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public long Pincode { get; set; }

        [Required]
        public long AadhaarCard { get; set; }

        [ForeignKey("Broker")]
        public int? BrokerId { get; set; }


        public virtual Broker Broker { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>(); // Initialize
    }

}
