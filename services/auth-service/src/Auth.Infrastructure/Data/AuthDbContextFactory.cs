using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace Auth.Infrastructure.Data;

public class AuthDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
{
    public AuthDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "..",
            "Auth.API");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString =
            configuration.GetConnectionString("AuthDb");

        var optionsBuilder =
            new DbContextOptionsBuilder<AuthDbContext>();

        optionsBuilder.UseNpgsql(connectionString);

        return new AuthDbContext(optionsBuilder.Options);
    }
}