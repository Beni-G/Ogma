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

        builder.OwnsOne(o => o.OrderPartner, partnerBuilder =>
        {
            partnerBuilder.Property(p => p.PartnerId)
                .HasColumnName("partner_id");

            partnerBuilder.Property(p => p.PartnerName)
                .HasColumnName("partner_name");
        });

        builder.OwnsMany(o => o.OrderLines, lineBuilder =>
        {
            lineBuilder.ToTable("order_lines");
            lineBuilder.HasKey(ol => ol.Id);
            lineBuilder.OwnsOne(ol => ol.OrderItem, itemBuilder =>
            {
                itemBuilder.Property(i => i.ItemId)
                    .HasColumnName("item_id");

                itemBuilder.Property(i => i.ItemName)
                    .HasColumnName("item_name");

                itemBuilder.Property(i => i.ItemCode)
                    .HasColumnName("item_code");
            });
        });
    }
}
