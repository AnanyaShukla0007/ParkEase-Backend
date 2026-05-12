using Analytics.Application.Interfaces;
using Analytics.Application.Services;
using Analytics.Infrastructure;

namespace Analytics.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAnalyticsService, AnalyticsService>();
        services.AddInfrastructure(configuration);
        return services;
    }
}
