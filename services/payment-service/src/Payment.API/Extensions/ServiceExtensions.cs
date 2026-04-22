using Payment.Application.Interfaces;
using Payment.Application.Services;
using Payment.Infrastructure;

namespace Payment.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddInfrastructure(configuration);

        return services;
    }
}
