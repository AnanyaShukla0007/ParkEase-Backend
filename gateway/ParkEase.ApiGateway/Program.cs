using ParkEase.ApiGateway.Extensions;
using ParkEase.ApiGateway.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile(
        "appsettings.json",
        optional: false,
        reloadOnChange: false)
    .AddJsonFile(
        $"appsettings.{builder.Environment.EnvironmentName}.json",
        optional: true,
        reloadOnChange: false)
    .AddJsonFile(
        "Routes/reverseproxy.json",
        optional: false,
        reloadOnChange: false)
    .AddJsonFile(
        $"Routes/reverseproxy.{builder.Environment.EnvironmentName}.json",
        optional: true,
        reloadOnChange: false)
    .AddEnvironmentVariables();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddGatewaySwagger();
builder.Services.AddGatewayCors();
builder.Services.AddGatewayReverseProxy(builder.Configuration);

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "ParkEase API Gateway v1");
    options.RoutePrefix = "swagger";
});


app.UseCors("GatewayCors");

app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        context.Response.Headers.Remove("Access-Control-Allow-Origin");
        context.Response.Headers.Remove("Access-Control-Allow-Credentials");
        context.Response.Headers.Remove("Access-Control-Allow-Headers");
        context.Response.Headers.Remove("Access-Control-Allow-Methods");
        context.Response.Headers.Remove("Access-Control-Expose-Headers");

        return Task.CompletedTask;
    });

    await next();
});

app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();

app.MapReverseProxy();

app.MapGet("/", () => Results.Redirect("/swagger"));

await app.RunAsync();
