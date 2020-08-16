using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseService.Entity;

namespace WarehouseService.Providers
{
    public class ReserveItemConfiguration : IEntityTypeConfiguration<ReserveItem>
    {
        public void Configure(EntityTypeBuilder<ReserveItem> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).ValueGeneratedOnAdd();

            builder.Property(i => i.ReserveId).IsRequired();
            
            builder.Property(i => i.ProductId).IsRequired();
            builder.HasIndex(i => i.ProductId);
            
            builder.Property(i => i.Quantity).IsRequired();
        }
    }
}