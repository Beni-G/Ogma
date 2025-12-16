using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Ogma.Domain.Partners.Repositories;
using Ogma.Infrastructure.Persistence.Partners.Contexts;
using Ogma.Infrastructure.Persistence.Partners.MappingProfiles;
using Ogma.Infrastructure.Persistence.Partners.Repositories;

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
        services.AddScoped<IPartnerRoleTypeRepository, PartnerRoleTypeRepository>();
        services.AddScoped<IPartnerRepository, PartnerRepository>();

        // AutoMapper Configuration.
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<DomainToPersistenceProfile>();
            cfg.AddExpressionMapping();
        }, AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }
}
