using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ogma.Infrastructure.Persistence.Orders.Models;

namespace Ogma.Infrastructure.Persistence.Orders.Configurations;

internal class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasOne(o => o.OrderType)
               .WithMany()
               .HasForeignKey(o => o.OrderTypeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.OrderStatus)
                .WithMany()
                .HasForeignKey(o => o.OrderStatusId)
                .OnDelete(DeleteBehavior.Restrict);
    }
}
