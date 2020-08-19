using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentService.Models;

namespace PaymentService.Infrastructure.Configurations
{
    public class InvoiceConfiguration: IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).ValueGeneratedOnAdd();
            
            builder.HasMany(i => i.CartItems)
                .WithOne()
                .HasForeignKey(i => i.InvoiceId);
            
            builder.Property(i => i.OrderId).IsRequired();
            builder.HasIndex(i => i.OrderId);
            
            builder.Property(i => i.ExternalOrderId).IsRequired();
            builder.Property(i => i.PaymentFormUrl).IsRequired();
            builder.Property(i => i.Paid).IsRequired().HasDefaultValue(false);
            builder.Property(i => i.CreationDate).IsRequired();
        }
    }
}