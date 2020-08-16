using DeliveryService.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryService.Providers
{
    public class DeliveryRequestConfiguration: IEntityTypeConfiguration<DeliveryRequest>
    {
        public void Configure(EntityTypeBuilder<DeliveryRequest> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            
            builder.Property(u => u.OrderId).IsRequired();
            builder.Property(u => u.DeliveryAddress).IsRequired();
            builder.Property(u => u.Recipient).IsRequired();
            builder.Property(u => u.PlannedDeliveryDate).IsRequired();
            builder.Property(u => u.CreationDate).IsRequired();
            
            builder.Property(u => u.Delivered)
                .IsRequired()
                .HasDefaultValue(false);
        }
    }
}