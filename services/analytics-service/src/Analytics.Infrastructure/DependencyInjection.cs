using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Analytics.Application.Interfaces;
using Analytics.Infrastructure.Clients;
using Analytics.Infrastructure.Persistence;
using Analytics.Infrastructure.Repositories;

namespace Analytics.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AnalyticsDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();

        services.AddHttpClient<IBookingAnalyticsClient, BookingAnalyticsClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["Services:BookingUrl"]!);
        });

        services.AddHttpClient<IPaymentAnalyticsClient, PaymentAnalyticsClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["Services:PaymentUrl"]!);
        });

        services.AddHttpClient<ISpotAnalyticsClient, SpotAnalyticsClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["Services:SpotUrl"]!);
        });

        services.AddHttpClient<IAuthAnalyticsClient, AuthAnalyticsClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["Services:AuthUrl"]!);
        });

        return services;
    }
}
