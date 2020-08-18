using Microsoft.EntityFrameworkCore;
using WarehouseService.Entity;
using WarehouseService.Infrastructure.Configurations;
using WarehouseService.Models;

namespace WarehouseService.Infrastructure
{
    public class WarehouseContext : DbContext
    {
        public DbSet<WarehouseItem> WarehouseItems { get; set; }
        public DbSet<Reserve> Reserves { get; set; }
        public DbSet<ReserveItem> ReserveItems { get; set; }

        public WarehouseContext(DbContextOptions<WarehouseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new WarehouseItemConfiguration());
            modelBuilder.ApplyConfiguration(new ReserveConfiguration());
            modelBuilder.ApplyConfiguration(new ReserveItemConfiguration());
        }
    }
}