using ParkEase.Web.Services.Clients;
using ParkEase.Web.Services.Interfaces;

namespace ParkEase.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddParkEaseApiClients(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient<IAuthApiClient, AuthApiClient>(client =>
            client.BaseAddress = new Uri(configuration["Services:Auth"]!));

        services.AddHttpClient<IParkingLotApiClient, ParkingLotApiClient>(client =>
            client.BaseAddress = new Uri(configuration["Services:ParkingLot"]!));

        services.AddHttpClient<ISpotApiClient, SpotApiClient>(client =>
            client.BaseAddress = new Uri(configuration["Services:Spot"]!));

        services.AddHttpClient<IBookingApiClient, BookingApiClient>(client =>
            client.BaseAddress = new Uri(configuration["Services:Booking"]!));

        services.AddHttpClient<IPaymentApiClient, PaymentApiClient>(client =>
            client.BaseAddress = new Uri(configuration["Services:Payment"]!));

        services.AddHttpClient<IVehicleApiClient, VehicleApiClient>(client =>
            client.BaseAddress = new Uri(configuration["Services:Vehicle"]!));

        services.AddHttpClient<INotificationApiClient, NotificationApiClient>(client =>
            client.BaseAddress = new Uri(configuration["Services:Notification"]!));

        services.AddHttpClient<IAnalyticsApiClient, AnalyticsApiClient>(client =>
            client.BaseAddress = new Uri(configuration["Services:Analytics"]!));

        return services;
    }
}
