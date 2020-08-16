using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Models;

namespace OrderService.Infrastructure.Configurations
{
    public class OrderConfiguration: IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).ValueGeneratedOnAdd();

            builder.HasMany(i => i.OrderItems)
                .WithOne()
                .HasForeignKey(i => i.OrderId);
            
            builder.Property(i => i.OrderStatusId).IsRequired();
            builder.Property(i => i.UserId).IsRequired();
            
            builder.Property(i => i.Paid).IsRequired().HasDefaultValue(false);
            
            builder.Property(i => i.TotalPrice).IsRequired().HasDefaultValue(0);
            
            builder.Property(i => i.CreationDate).IsRequired();
            builder.Property(i => i.UpdateDate).IsRequired();
        }
    }
}