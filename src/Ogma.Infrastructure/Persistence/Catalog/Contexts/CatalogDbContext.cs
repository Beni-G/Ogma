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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
        modelBuilder.HasAnnotation("Relational:MigrationHistoryTable", "__CatalogMigrationsHistory");
    }

}
