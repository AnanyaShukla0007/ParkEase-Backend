using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spot.Application.Interfaces;
using Spot.Application.Services;
using ParkEase.Spot.Infrastructure.Persistence;
using ParkEase.Spot.Infrastructure.Repositories;

namespace ParkEase.Spot.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddSpotInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<SpotDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ISpotRepository, SpotRepository>();
        services.AddScoped<ISpotService, SpotServices>();

        return services;
    }
}