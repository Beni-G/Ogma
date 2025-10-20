using Microsoft.Extensions.DependencyInjection;
using Ogma.Application.Catalog.Services;
using Ogma.Domain.Catalog.Services;
using System.Reflection;

namespace Ogma.Application.Extensions;
public static class ApplicationConfigurationExtension
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddScoped<ICategoryDomainService, CategoryDomainService>();

        return services;
    }
}
