using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Auth.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AuthDbContext _context;
    private readonly UserManager<User> _userManager;

    public UserRepository(
        AuthDbContext context,
        UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<User?> FindByEmailAsync(string email)
        => await _userManager.FindByEmailAsync(email);

    public async Task<User?> FindByIdAsync(int userId)
        => await _userManager.FindByIdAsync(userId.ToString());

    public async Task<User?> FindByPhoneAsync(string phone)
        => await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phone);

    public async Task<User?> FindByVehiclePlateAsync(string plate)
        => await _context.Users.FirstOrDefaultAsync(x => x.VehiclePlate == plate);

    public async Task<List<User>> FindAllByRoleAsync(string role)
        => await _context.Users
            .Where(x => x.Role == role)
            .ToListAsync();

    public async Task<bool> ExistsByEmailAsync(string email)
        => await _context.Users.AnyAsync(x => x.Email == email);

    public async Task<bool> ExistsByPhoneAsync(string phone)
        => await _context.Users.AnyAsync(x => x.PhoneNumber == phone);

    public async Task UpdateAsync(User user)
    {
        await _userManager.UpdateAsync(user);
    }

    public async Task DeleteAsync(int userId)
    {
        var user = await FindByIdAsync(userId);
        if (user is not null)
            await _userManager.DeleteAsync(user);
    }

    public async Task<List<User>> GetAllAsync()
        => await _context.Users.ToListAsync();
}