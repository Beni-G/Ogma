using Microsoft.EntityFrameworkCore;
using Ogma.Infrastructure.Persistence.Orders.Models;

namespace Ogma.Infrastructure.Persistence.Orders.Contexts;

public class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options)
        : base(options) { }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderType> OrderTypes => Set<OrderType>();
    public DbSet<OrderStatus> OrderStatuses => Set<OrderStatus>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(OrdersDbContext).Assembly,
            type => type.Namespace is not null
                    && type.Namespace.StartsWith("Ogma.Infrastructure.Persistence.Orders", StringComparison.Ordinal));
        modelBuilder.HasAnnotation("Relational:MigrationHistoryTable", "__OrdersMigrationsHistory");
    }
}
