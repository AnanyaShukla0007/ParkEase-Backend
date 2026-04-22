using System.Text.Json;

namespace Notification.API.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (KeyNotFoundException ex)
        {
            await WriteResponseAsync(context, StatusCodes.Status404NotFound, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            await WriteResponseAsync(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            await WriteResponseAsync(context, StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    private static async Task WriteResponseAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            success = false,
            message
        }));
    }
}
