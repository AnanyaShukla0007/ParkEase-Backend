using Booking.Application.Interfaces;
using Booking.Application.Services;
using Booking.Infrastructure;

namespace Booking.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IBookingService, BookingService>();

        services.AddInfrastructure(configuration);

        return services;
    }
}