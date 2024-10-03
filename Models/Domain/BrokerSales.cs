using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PropertySales.Models.Domain;

namespace Propertycheck.Models.Domain
{
    public class BrokerSales
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SaleId   { get; set; }

        [Required]
        [ForeignKey("Broker")]
        public int BrokerId { get; set; }
        public virtual Broker Broker { get; set; }

        [Required]
        [ForeignKey("Transaction")]
        public int TransactionId { get; set; }

        public virtual Transaction Transaction { get; set; }

        [Required]
        public int Commission { get; set; }


    }
}
