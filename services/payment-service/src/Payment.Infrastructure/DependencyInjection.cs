using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payment.Application.Interfaces;
using Payment.Infrastructure.Clients;
using Payment.Infrastructure.Persistence;
using Payment.Infrastructure.Repositories;

namespace Payment.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<PaymentDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Payment.Infrastructure")));

        services.AddScoped<IPaymentRepository, PaymentRepository>();

        services.AddHttpClient<IBookingPaymentClient, BookingPaymentClient>(client =>
        {
            var bookingUrl = configuration["Services:BookingUrl"];
            if (string.IsNullOrWhiteSpace(bookingUrl) ||
                bookingUrl.Contains("booking-service", StringComparison.OrdinalIgnoreCase))
            {
                bookingUrl = "https://parkease-booking-rcdk.onrender.com";
            }

            client.BaseAddress = new Uri(bookingUrl);
        });

        return services;
    }
}
