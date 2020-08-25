using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Models;

namespace OrderService.Infrastructure.Configurations
{
    public class IdempotencyKeyConfiguration: IEntityTypeConfiguration<IdempotencyKey>
    {
        public void Configure(EntityTypeBuilder<IdempotencyKey> builder)
        {
            builder.HasKey(i => i.Key);

            builder.Property(i => i.CreationDate).IsRequired();
        }
    }
}