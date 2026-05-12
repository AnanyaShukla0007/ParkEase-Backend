using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ParkingLot.Application.Interfaces;
using ParkingLot.Infrastructure.Persistence;
using ParkingLot.Infrastructure.Repositories;

namespace ParkingLot.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ParkingLotDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IParkingLotRepository, ParkingLotRepository>();

        return services;
    }
}