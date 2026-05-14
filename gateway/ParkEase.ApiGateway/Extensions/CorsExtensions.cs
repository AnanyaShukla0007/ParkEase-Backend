namespace ParkEase.ApiGateway.Extensions;

public static class CorsExtensions
{
    public static IServiceCollection AddGatewayCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("GatewayCors", policy =>
            {
                policy.SetIsOriginAllowed(origin =>
                      {
                          if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri))
                              return false;

                          return uri.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase)
                                 || uri.Host.EndsWith(".vercel.app", StringComparison.OrdinalIgnoreCase);
                      })
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });

        return services;
    }
}
