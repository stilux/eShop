using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Models;

namespace OrderService.Infrastructure.Configurations
{
    public class PaymentMethodConfiguration: IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).ValueGeneratedOnAdd();
            builder.HasMany<Order>()
                .WithOne(i => i.PaymentMethod)
                .HasForeignKey(i => i.PaymentMethodId);
            
            builder.Property(i => i.Name).IsRequired();
        }
    }
}