using Auth.Domain.Entities;

namespace Auth.Application.Interfaces;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> FindByTokenAsync(string token);

    Task<List<RefreshToken>> FindByUserIdAsync(int userId);

    Task AddAsync(RefreshToken token);

    Task RevokeAsync(string token);

    Task RevokeAllByUserIdAsync(int userId);

    Task DeleteExpiredAsync();
}