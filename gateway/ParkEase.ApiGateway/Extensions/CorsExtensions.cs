namespace ParkEase.ApiGateway.Extensions;

public static class CorsExtensions
{
    public static IServiceCollection AddGatewayCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("GatewayCors", policy =>
            {
                policy.WithOrigins("http://localhost:4200")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });

        return services;
    }
}