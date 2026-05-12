using Yarp.ReverseProxy;

namespace ParkEase.ApiGateway.Extensions;

public static class ReverseProxyExtensions
{
    public static IServiceCollection AddGatewayReverseProxy(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddReverseProxy()
            .LoadFromConfig(configuration.GetSection("ReverseProxy"));

        return services;
    }
}