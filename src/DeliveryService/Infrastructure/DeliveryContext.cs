using DeliveryService.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Infrastructure
{
    public class DeliveryContext : DbContext
    {
        public DbSet<DeliveryRequest> DeliveryRequests { get; set; }

        public DeliveryContext(DbContextOptions<DeliveryContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new DeliveryRequestConfiguration());
        }
    }
}