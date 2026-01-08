using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ogma.Infrastructure.Persistence.Catalog.Models;

namespace Ogma.Infrastructure.Persistence.Catalog.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.HasOne(i => i.Category)
               .WithMany()
               .HasForeignKey(i => i.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.ItemType)
                .WithMany()
                .HasForeignKey(i => i.ItemTypeId)
                .OnDelete(DeleteBehavior.Restrict);
    }
}
