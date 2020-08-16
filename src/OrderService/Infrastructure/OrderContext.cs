using Microsoft.EntityFrameworkCore;
using OrderService.Infrastructure.Configurations;
using OrderService.Models;

namespace OrderService.Providers
{
    public class OrderContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        
        public DbSet<OrderItem> OrderItems { get; set; }

        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
            modelBuilder.ApplyConfiguration(new OrderStatusConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentMethodConfiguration());
        }
    }
}