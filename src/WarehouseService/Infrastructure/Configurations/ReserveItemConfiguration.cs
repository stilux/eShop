using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseService.Entity;
using WarehouseService.Models;

namespace WarehouseService.Infrastructure.Configurations
{
    public class ReserveItemConfiguration : IEntityTypeConfiguration<ReserveItem>
    {
        public void Configure(EntityTypeBuilder<ReserveItem> builder)
        {
            builder.HasKey(i => new { i.ReserveId, i.ProductId });

            builder.Property(i => i.ReserveId).IsRequired();
            builder.HasIndex(i => i.ReserveId);
            
            builder.Property(i => i.ProductId).IsRequired();
           
            builder.Property(i => i.Quantity).IsRequired();
        }
    }
}