using ParkingLot.Application.Interfaces;
using ParkingLot.Application.Services;

namespace ParkingLot.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddScoped<IParkingLotService, ParkingLotService>();
        return services;
    }
}