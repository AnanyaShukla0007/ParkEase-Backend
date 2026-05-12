using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Infrastructure.Security;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateAccessToken(User user)
    {
        var jwt = _configuration.GetSection("JwtSettings");

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt["SecretKey"]!));

        var creds = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new("userId", user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email ?? ""),
            new(ClaimTypes.Role, user.Role),
            new("fullName", user.FullName)
        };

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: GetAccessTokenExpiry(),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }

    public bool ValidateToken(string token)
        => ValidatePrincipal(token) is not null;

    public int? GetUserId(string token)
    {
        var principal = ValidatePrincipal(token);

        var value =
            principal?.FindFirst("userId")?.Value ??
            principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return int.TryParse(value, out var id) ? id : null;
    }

    public DateTime GetRefreshTokenExpiry()
    {
        var days = int.Parse(
            _configuration["JwtSettings:RefreshTokenExpiryDays"] ?? "7");

        return DateTime.UtcNow.AddDays(days);
    }

    public DateTime GetAccessTokenExpiry()
    {
        var mins = int.Parse(
            _configuration["JwtSettings:AccessTokenExpiryMinutes"] ?? "60");

        return DateTime.UtcNow.AddMinutes(mins);
    }

    private ClaimsPrincipal? ValidatePrincipal(string token)
    {
        try
        {
            var jwt = _configuration.GetSection("JwtSettings");

            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwt["SecretKey"]!)),

                ValidateIssuer = true,
                ValidIssuer = jwt["Issuer"],

                ValidateAudience = true,
                ValidAudience = jwt["Audience"],

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            return new JwtSecurityTokenHandler()
                .ValidateToken(token, parameters, out _);
        }
        catch
        {
            return null;
        }
    }
}