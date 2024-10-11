using Microsoft.EntityFrameworkCore;
using PropertySales.Models.Domain;

namespace PropertySales.Data
{
    public class PropertySalesDbContext : DbContext
    {
        public PropertySalesDbContext(DbContextOptions<PropertySalesDbContext> options) : base(options) { }

        public DbSet<Property> Properties { get; set; }
      
    }
}

