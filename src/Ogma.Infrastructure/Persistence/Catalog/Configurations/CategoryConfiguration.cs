using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ogma.Infrastructure.Persistence.Catalog.Models;

namespace Ogma.Infrastructure.Persistence.Catalog.Configurations;
internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasOne(c => c.ParentCategory)
           .WithMany(c => c.SubCategories)
           .HasForeignKey(c => c.ParentCategoryId)
           .OnDelete(DeleteBehavior.Restrict);
    }
}
