using Microsoft.EntityFrameworkCore;
using Ogma.Infrastructure.Persistence.Partners.Models;

namespace Ogma.Infrastructure.Persistence.Partners.Contexts;

public class PartnersDbContext : DbContext
{
    public PartnersDbContext(DbContextOptions<PartnersDbContext> options)
        : base(options) { }

    public DbSet<Partner> Partners => Set<Partner>();
    public DbSet<PartnerRoleType> PartnerRoleTypes => Set<PartnerRoleType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(PartnersDbContext).Assembly,
            type => type.Namespace is not null
                    && type.Namespace.StartsWith("Ogma.Infrastructure.Persistence.Partners", System.StringComparison.Ordinal));

        modelBuilder.HasAnnotation("Relational:MigrationHistoryTable", "__PartnersMigrationsHistory");
    }

    
}
