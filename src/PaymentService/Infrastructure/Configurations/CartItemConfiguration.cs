using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentService.Models;

namespace PaymentService.Infrastructure.Configurations
{
    public class CartItemConfiguration: IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(i => new { i.InvoiceId, i.PositionId });
            
            builder.Property(i => i.Name).IsRequired();
            
            builder.HasIndex(i => i.InvoiceId);
            
            builder.Property(i => i.Quantity).IsRequired();
            builder.Property(i => i.ItemCode).IsRequired();
            builder.Property(i => i.ItemAmount).IsRequired();
        }
    }
}