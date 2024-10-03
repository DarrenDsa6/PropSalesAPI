using Microsoft.EntityFrameworkCore;
using Propertycheck.Models.Domain;
using PropertySales.Models.Domain;

namespace PropertySales.Data
{
    public class PropertySalesDbContext : DbContext
    {
        public PropertySalesDbContext(DbContextOptions<PropertySalesDbContext> options) : base(options) { }

        public DbSet<Broker> Brokers { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<BrokerSales> BrokerSales { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Property and Transaction - prevent cascading delete
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Property)
                .WithMany(p => p.Transactions)
                .HasForeignKey(t => t.PropertyId)
                .OnDelete(DeleteBehavior.NoAction);

            // Broker and Transaction - prevent cascading delete
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Broker)
                .WithMany(b => b.Transactions)
                .HasForeignKey(t => t.BrokerId)
                .OnDelete(DeleteBehavior.NoAction);

            // User and Transaction - prevent cascading delete
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.BuyerId)
                .OnDelete(DeleteBehavior.NoAction);

            // Broker and User - prevent cascading delete
            modelBuilder.Entity<User>()
                .HasOne(u => u.Broker)
                .WithMany(b => b.Users) // Assuming Broker has a collection of Users
                .HasForeignKey(u => u.BrokerId)
                .OnDelete(DeleteBehavior.NoAction);

            // Property and BrokerSales - prevent cascading delete
            modelBuilder.Entity<BrokerSales>()
                .HasOne(bs => bs.Broker)
                .WithMany(b => b.BrokerSales) // Assuming Broker has a collection of BrokerSales
                .HasForeignKey(bs => bs.BrokerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<BrokerSales>()
                .HasOne(bs => bs.Transaction)
                .WithMany(t => t.BrokerSales) // Assuming Transaction has a collection of BrokerSales
                .HasForeignKey(bs => bs.TransactionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

