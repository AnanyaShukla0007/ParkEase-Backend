using Booking.Application.Interfaces;
using Booking.Infrastructure.Clients;
using Booking.Infrastructure.Persistence;
using Booking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<BookingDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IBookingRepository, BookingRepository>();

        services.AddHttpClient<ISpotClient, SpotClient>(client =>
        {
            client.BaseAddress =
                new Uri(configuration["Services:SpotUrl"]!);
        });

        services.AddHttpClient<IParkingLotClient, ParkingLotClient>(client =>
        {
            client.BaseAddress =
                new Uri(configuration["Services:ParkingLotUrl"]!);
        });

        return services;
    }
}