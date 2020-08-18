using DeliveryService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryService.Infrastructure
{
    public class DeliveryRequestConfiguration: IEntityTypeConfiguration<DeliveryRequest>
    {
        public void Configure(EntityTypeBuilder<DeliveryRequest> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).ValueGeneratedOnAdd();
            
            builder.Property(i => i.OrderId).IsRequired();
            builder.HasIndex(i => i.OrderId);
            
            builder.Property(i => i.DeliveryAddress).IsRequired();
            builder.Property(i => i.Recipient).IsRequired();
            builder.Property(i => i.DeliveryDate).IsRequired();
            builder.Property(i => i.CreationDate).IsRequired();
            
            builder.Property(i => i.Delivered)
                .IsRequired()
                .HasDefaultValue(false);
        }
    }
}