using Microsoft.EntityFrameworkCore;
using Ogma.Infrastructure.Persistence.Catalog.Models;

namespace Ogma.Infrastructure.Persistence.Catalog.Contexts;
public class CatalogDbContext : DbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
        : base(options) { }

    public DbSet<Item> Items => Set<Item>();
    public DbSet<ItemType> ItemTypes => Set<ItemType>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(CatalogDbContext).Assembly,
            type => type.Namespace is not null
                    && type.Namespace.StartsWith("Ogma.Infrastructure.Persistence.Catalog", System.StringComparison.Ordinal));

        modelBuilder.HasAnnotation("Relational:MigrationHistoryTable", "__CatalogMigrationsHistory");
    }

}
