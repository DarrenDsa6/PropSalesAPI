using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Propertycheck.Models.Domain;

namespace PropertySales.Models.Domain
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        [ForeignKey("Property")]
        public int PropertyId { get; set; }

        public virtual Property Property { get; set; }

        [ForeignKey("User")]
        public int BuyerId { get; set; }

        [ForeignKey("Broker")]
        public int BrokerId { get; set; }

        public virtual Broker Broker { get; set; }

        public DateTime TransactionDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [Column(TypeName = "varchar(10)")]
        public TransactionStatus Status { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<BrokerSales> BrokerSales { get; set; } = new List<BrokerSales>(); // Initialize

    }

    public enum TransactionType
    {
        Sale,
        Rent
    }

    public enum TransactionStatus
    {
        Pending,
        Completed,
        Cancelled
    }

}
