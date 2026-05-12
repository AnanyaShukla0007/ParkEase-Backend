using ParkEase.ApiGateway.Extensions;
using ParkEase.ApiGateway.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
    .AddJsonFile("Routes/reverseproxy.json", optional: false, reloadOnChange: true)
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

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "ParkEase API Gateway v1");
    options.RoutePrefix = "swagger";
});

// app.UseHttpsRedirection();
app.UseCors("GatewayCors");

app.MapControllers();
app.MapReverseProxy();
app.MapGet("/", () => Results.Redirect("/swagger"));
app.Run();