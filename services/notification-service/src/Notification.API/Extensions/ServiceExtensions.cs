using Notification.API.Realtime;
using Notification.Application.Interfaces;
using Notification.Application.Services;
using Notification.Infrastructure;

namespace Notification.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<INotificationRealtimeDispatcher, SignalRNotificationRealtimeDispatcher>();
        services.AddInfrastructure(configuration);

        return services;
    }
}
