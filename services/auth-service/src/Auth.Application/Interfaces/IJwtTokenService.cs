namespace Auth.Application.Interfaces;

public interface IJwtTokenService
{
    string GenerateAccessToken(Auth.Domain.Entities.User user);

    string GenerateRefreshToken();

    bool ValidateToken(string token);

    int? GetUserId(string token);

    DateTime GetRefreshTokenExpiry();

    DateTime GetAccessTokenExpiry();
}