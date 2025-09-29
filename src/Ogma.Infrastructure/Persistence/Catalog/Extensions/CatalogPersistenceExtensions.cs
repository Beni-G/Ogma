using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ogma.Domain.Catalog.Repositories;
using Ogma.Infrastructure.Persistence.Catalog.Contexts;
using Ogma.Infrastructure.Persistence.Catalog.Repositories;

namespace Ogma.Infrastructure.Persistence.Catalog.Extensions;
public static class CatalogPersistenceExtensions
{
    public static IServiceCollection AddCatalogPersistence(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("PostgresMain");

        services.AddDbContext<CatalogDbContext>(options =>
            options.UseNpgsql(connectionString)
                   .UseSnakeCaseNamingConvention());

        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IItemTypeRepository, ItemTypeRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        return services;
    }
}
