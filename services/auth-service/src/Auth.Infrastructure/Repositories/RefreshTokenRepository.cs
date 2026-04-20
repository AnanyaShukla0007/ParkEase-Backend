using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Auth.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AuthDbContext _context;

    public RefreshTokenRepository(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken?> FindByTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Token == token);
    }

    public async Task<List<RefreshToken>> FindByUserIdAsync(int userId)
    {
        return await _context.RefreshTokens
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task AddAsync(RefreshToken token)
    {
        await _context.RefreshTokens.AddAsync(token);
        await _context.SaveChangesAsync();
    }

    public async Task RevokeAsync(string token)
    {
        var item = await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == token);

        if (item is null) return;

        item.IsRevoked = true;
        await _context.SaveChangesAsync();
    }

    public async Task RevokeAllByUserIdAsync(int userId)
    {
        var items = await _context.RefreshTokens
            .Where(x => x.UserId == userId && !x.IsRevoked)
            .ToListAsync();

        foreach (var item in items)
            item.IsRevoked = true;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteExpiredAsync()
    {
        var items = await _context.RefreshTokens
            .Where(x => x.IsRevoked || x.ExpiresAt < DateTime.UtcNow)
            .ToListAsync();

        _context.RefreshTokens.RemoveRange(items);
        await _context.SaveChangesAsync();
    }
}