using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Auth.Infrastructure.Data;
using Auth.Infrastructure.Repositories;
using Auth.Infrastructure.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("AuthDb");

        services.AddDbContext<AuthDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddIdentity<User, IdentityRole<int>>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<AuthDbContext>()
        .AddDefaultTokenProviders();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<JwtTokenService>();

        return services;
    }
}