using Microsoft.EntityFrameworkCore;
using PaymentService.Infrastructure.Configurations;
using PaymentService.Models;

namespace PaymentService.Infrastructure
{
    public class PaymentContext : DbContext
    {
        public DbSet<Invoice> Invoices { get; set; }

        public PaymentContext(DbContextOptions<PaymentContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
            modelBuilder.ApplyConfiguration(new CartItemConfiguration());
        }
    }
}