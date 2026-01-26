using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Ogma.Application.Orders.Ports;
using Ogma.Domain.Orders.Repositories;
using Ogma.Infrastructure.Persistence.Orders.Adapters;
using Ogma.Infrastructure.Persistence.Orders.Contexts;
using Ogma.Infrastructure.Persistence.Orders.MappingProfiles;
using Ogma.Infrastructure.Persistence.Orders.Repositories;

namespace Ogma.Infrastructure.Persistence.Orders.Extensions;

public static class OrderPersistenceExtensions
{
    public static IServiceCollection AddOrdersPersistence(this IServiceCollection services, IConfiguration config)
    {
        // Database Context Configuration.
        var connectionString = config.GetConnectionString("PostgresMain");

        services.AddDbContext<OrdersDbContext>(options =>
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
            dataSourceBuilder.EnableDynamicJson();

            var dataSource = dataSourceBuilder.Build();

            options.UseNpgsql(dataSource)
                   .UseSnakeCaseNamingConvention();
        });

        // Repository and Readers Registrations.
        services.AddScoped<ICatalogItemReader, CatalogItemReader>();
        services.AddScoped<IPartnerReader, PartnerReader>();
        services.AddScoped<IOrderTypeRepository, OrderTypeRepository>();
        services.AddScoped<IOrderTypeReader, OrderTypeReader>();
        services.AddScoped<IOrderStatusRepository, OrderStatusRepository>();
        services.AddScoped<IOrderStatusReader, OrderStatusReader>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderReader, OrderReader>();

        // AutoMapper Configuration.
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<DomainToPersistenceProfile>();
            cfg.AddExpressionMapping();
        }, AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }
}
