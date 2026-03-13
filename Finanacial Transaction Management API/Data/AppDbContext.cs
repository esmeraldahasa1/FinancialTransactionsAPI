using Finanacial_Transaction_Management_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialTransactionAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                .HasKey(t => t.TransactionId);

            modelBuilder.Entity<Customer>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Customer)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CustomerId);

            // SQL INDEX
            modelBuilder.Entity<Transaction>()
                .HasIndex(t => t.CustomerId);

            modelBuilder.Entity<Transaction>()
                .HasIndex(t => t.TransactionDate);

            modelBuilder.Entity<Transaction>()
                .HasIndex(t => new { t.CustomerId, t.TransactionDate });
        }
    }
}