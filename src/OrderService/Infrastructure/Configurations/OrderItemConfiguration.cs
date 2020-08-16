using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Models;

namespace OrderService.Infrastructure.Configurations
{
    public class OrderItemConfiguration: IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(i => new { i.OrderId, i.ProductId});
            
            builder.Property(i => i.OrderId).IsRequired();
            builder.HasIndex(i => i.OrderId);

            builder.Property(i => i.ProductId).IsRequired();
            builder.Property(i => i.Quantity).IsRequired().HasDefaultValue(0);
        }
    }
}