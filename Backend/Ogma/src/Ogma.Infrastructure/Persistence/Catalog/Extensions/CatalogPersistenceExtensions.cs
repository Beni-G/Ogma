using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ogma.Domain.Catalog.Repositories;
using Ogma.Infrastructure.Persistence.Catalog.Contexts;
using Ogma.Infrastructure.Persistence.Catalog.MappingProfiles;
using Ogma.Infrastructure.Persistence.Catalog.Repositories;

namespace Ogma.Infrastructure.Persistence.Catalog.Extensions;

public static class CatalogPersistenceExtensions
{
    public static IServiceCollection AddCatalogPersistence(this IServiceCollection services, IConfiguration config)
    {
        // Database Context Configuration.
        var connectionString = config.GetConnectionString("PostgresMain");

        services.AddDbContext<CatalogDbContext>(options =>
            options.UseNpgsql(connectionString)
                   .UseSnakeCaseNamingConvention());

        // Repository Registrations.
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IItemTypeRepository, ItemTypeRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        // AutoMapper Configuration.
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<DomainToPersistenceProfile>();
            cfg.AddExpressionMapping();
        }, AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }
}
