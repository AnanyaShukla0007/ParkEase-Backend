using Vehicle.Application.Interfaces;
using Vehicle.Application.Services;
using Vehicle.Infrastructure;

namespace Vehicle.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddInfrastructure(configuration);

        return services;
    }
}
