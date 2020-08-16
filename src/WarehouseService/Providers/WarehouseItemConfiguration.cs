using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseService.Entity;

namespace WarehouseService.Providers
{
    public class WarehouseItemConfiguration: IEntityTypeConfiguration<WarehouseItem>
    {
        public void Configure(EntityTypeBuilder<WarehouseItem> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).ValueGeneratedOnAdd();
            
            builder.Property(i => i.ProductId).IsRequired();
            builder.HasIndex(i => i.ProductId).IsUnique();
            
            builder.Property(i => i.Total).IsRequired();
            builder.Property(i => i.Total).HasDefaultValue(0);
            
            builder.Property(i => i.ReservedQuantity).IsRequired();
            builder.Property(i => i.ReservedQuantity).HasDefaultValue(0);
        }
    }
}