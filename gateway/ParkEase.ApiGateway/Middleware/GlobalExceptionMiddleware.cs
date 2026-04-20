using System.Text.Json;

namespace ParkEase.ApiGateway.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var json = JsonSerializer.Serialize(new
            {
                success = false,
                message = "Gateway error occurred."
            });

            await context.Response.WriteAsync(json);
        }
    }
}