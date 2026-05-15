using Booking.API.Extensions;
using Booking.API.Middlewares;
using Booking.Infrastructure.Persistence;
using Booking.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;
using Booking.API.Filters;
Environment.SetEnvironmentVariable("DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE", "false");
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ValidationFilter>();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddSwaggerDocs();
builder.Services.AddCorsPolicy();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Booking Service v1");
    c.RoutePrefix = "swagger";
});
app.UseCors("AllowAll");

app.MapControllers();

app.MapGet("/", () => Results.Redirect("/swagger"));

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BookingDbContext>();

    await db.Database.MigrateAsync();
    await BookingSeeder.SeedAsync(db);
}

app.Run();
