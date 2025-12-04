using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Ogma.Infrastructure.Persistence.Partners.Contexts;

namespace Ogma.Infrastructure.Persistence.Partners.Extensions;

public static class PartnersPersistenceExtensions
{
    public static IServiceCollection AddPartnersPersistence(this IServiceCollection services, IConfiguration config)
    {
        // Database Context Configuration.
        var connectionString = config.GetConnectionString("PostgresMain");

        services.AddDbContext<PartnersDbContext>(options =>
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
            dataSourceBuilder.EnableDynamicJson();  

            var dataSource = dataSourceBuilder.Build();

            options.UseNpgsql(dataSource)
                   .UseSnakeCaseNamingConvention();
        });

        // Repository Registrations.

        // AutoMapper Configuration.

        return services;
    }
}
