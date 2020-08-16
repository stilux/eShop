using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Models;

namespace OrderService.Infrastructure.Configurations
{
    public class OrderStatusConfiguration: IEntityTypeConfiguration<OrderStatus>
    {
        public void Configure(EntityTypeBuilder<OrderStatus> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).ValueGeneratedOnAdd();
            builder.HasMany<Order>()
                .WithOne(i => i.OrderStatus)
                .HasForeignKey(i => i.OrderStatusId);
            
            builder.Property(i => i.Name).IsRequired();
        }
    }
}