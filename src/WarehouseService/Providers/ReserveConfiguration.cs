using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseService.Entity;

namespace WarehouseService.Providers
{
    public class ReserveConfiguration : IEntityTypeConfiguration<Reserve>
    {
        public void Configure(EntityTypeBuilder<Reserve> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).ValueGeneratedOnAdd();
            
            builder.Property(i => i.CreationDate).IsRequired();

            builder.HasMany(i => i.ReserveItems)
                .WithOne(i => i.Reserve)
                .HasForeignKey(i => i.ReserveId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}