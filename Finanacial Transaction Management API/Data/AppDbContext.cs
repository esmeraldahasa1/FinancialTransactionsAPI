
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
        public DbSet<CustomerPhone> CustomerPhones { get; set; }
        public DbSet<CustomerEmail> CustomerEmails { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           // Celesi primar 
            modelBuilder.Entity<Transaction>()
                .HasKey(t => t.TransactionId);

            modelBuilder.Entity<Customer>()
                .HasKey(c => c.Id);

            // Transaksioni : Klienti (Many-to-One)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Customer)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CustomerId)
                .OnDelete(DeleteBehavior.Restrict); // Nuk fshihet transaksioni kur fshihet klienti

            // Klienti : Numrat e telefonit (One-to-Many)
            modelBuilder.Entity<CustomerPhone>()
                .HasOne(p => p.Customer)
                .WithMany(c => c.Phones)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Cascade); // Fshij numrat kur fshihet klienti

            // Klienti dhe emailet (One-to-Many)
            modelBuilder.Entity<CustomerEmail>()
                .HasOne(e => e.Customer)
                .WithMany(c => c.Emails)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Klienti dhe adresat 
            modelBuilder.Entity<CustomerAddress>()
                .HasOne(a => a.Customer)
                .WithMany(c => c.Addresses)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indekset per performance
            modelBuilder.Entity<Transaction>()
                .HasIndex(t => t.TransactionDate);

            modelBuilder.Entity<Transaction>()
                .HasIndex(t => t.CustomerId);

            modelBuilder.Entity<Transaction>()
                .HasIndex(t => t.IsDeleted);

            modelBuilder.Entity<CustomerPhone>()
                .HasIndex(p => new { p.CustomerId, p.IsDeleted });

            modelBuilder.Entity<CustomerEmail>()
                .HasIndex(e => new { e.CustomerId, e.IsDeleted });

            modelBuilder.Entity<CustomerAddress>()
                .HasIndex(a => new { a.CustomerId, a.IsDeleted });

            // Global query filter per soft delete 
            modelBuilder.Entity<Transaction>().HasQueryFilter(t => !t.IsDeleted);
            modelBuilder.Entity<Customer>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<CustomerPhone>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<CustomerEmail>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<CustomerAddress>().HasQueryFilter(a => !a.IsDeleted);

            // Konfigurim shtese
            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2); // Precision per decimal

            // Rregullat per te siguruar qe vetem nje kontakt kryesor per klient
            modelBuilder.Entity<CustomerPhone>()
                .HasIndex(p => new { p.CustomerId, p.IsMain })
                .HasFilter("IsMain = 1 AND IsDeleted = 0")
                .IsUnique();

            modelBuilder.Entity<CustomerEmail>()
                .HasIndex(e => new { e.CustomerId, e.IsMain })
                .HasFilter("IsMain = 1 AND IsDeleted = 0")
                .IsUnique();

            modelBuilder.Entity<CustomerAddress>()
                .HasIndex(a => new { a.CustomerId, a.IsMain })
                .HasFilter("IsMain = 1 AND IsDeleted = 0")
                .IsUnique();
        }
    }
}