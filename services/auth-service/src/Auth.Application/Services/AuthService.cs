using Auth.Application.DTOs;
using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Auth.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly IRefreshTokenRepository _tokens;
    private readonly IJwtTokenService _jwt;
    private readonly UserManager<User> _userManager;

    public AuthService(
        IUserRepository users,
        IRefreshTokenRepository tokens,
        IJwtTokenService jwt,
        UserManager<User> userManager)
    {
        _users = users;
        _tokens = tokens;
        _jwt = jwt;
        _userManager = userManager;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _users.ExistsByEmailAsync(request.Email))
            throw new InvalidOperationException("Email already registered.");

        if (await _users.ExistsByPhoneAsync(request.PhoneNumber))
            throw new InvalidOperationException("Phone already registered.");

        var user = new User
        {
            FullName = request.FullName.Trim(),
            Email = request.Email.Trim().ToLower(),
            UserName = request.Email.Trim().ToLower(),
            PhoneNumber = request.PhoneNumber,
            Role = request.Role.ToUpper(),
            VehiclePlate = request.VehiclePlate,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            throw new InvalidOperationException(
                string.Join(", ", result.Errors.Select(x => x.Description)));

        await _userManager.AddToRoleAsync(user, user.Role);

        return await BuildAuthResponse(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _users.FindByEmailAsync(request.Email)
                   ?? throw new UnauthorizedAccessException("Invalid credentials.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Account disabled.");

        var valid = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!valid)
            throw new UnauthorizedAccessException("Invalid credentials.");

        return await BuildAuthResponse(user);
    }

    public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
    {
        var token = await _tokens.FindByTokenAsync(refreshToken);

        if (token is null || token.IsRevoked || token.ExpiresAt < DateTime.UtcNow)
            throw new UnauthorizedAccessException("Invalid refresh token.");

        await _tokens.RevokeAsync(refreshToken);

        return await BuildAuthResponse(token.User);
    }

    public async Task LogoutAsync(int userId, string refreshToken)
    {
        await _tokens.RevokeAsync(refreshToken);
    }

    public async Task<UserResponse> GetUserByIdAsync(int userId)
    {
        var user = await _users.FindByIdAsync(userId)
                   ?? throw new KeyNotFoundException("User not found.");

        return Map(user);
    }

    public async Task<List<UserResponse>> GetAllUsersAsync()
    {
        var users = await _users.GetAllAsync();
        return users.Select(Map).ToList();
    }

    public async Task<List<UserResponse>> GetUsersByRoleAsync(string role)
    {
        var users = await _users.FindAllByRoleAsync(role);
        return users.Select(Map).ToList();
    }

    public async Task<UserResponse> UpdateProfileAsync(int userId, UpdateProfileRequest request)
    {
        var user = await _users.FindByIdAsync(userId)
                   ?? throw new KeyNotFoundException("User not found.");

        if (!string.IsNullOrWhiteSpace(request.FullName))
            user.FullName = request.FullName;

        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            user.PhoneNumber = request.PhoneNumber;

        if (request.VehiclePlate is not null)
            user.VehiclePlate = request.VehiclePlate;

        if (request.ProfilePicUrl is not null)
            user.ProfilePicUrl = request.ProfilePicUrl;

        await _users.UpdateAsync(user);

        return Map(user);
    }

    public async Task ChangePasswordAsync(int userId, ChangePasswordRequest request)
    {
        var user = await _users.FindByIdAsync(userId)
                   ?? throw new KeyNotFoundException("User not found.");

        var result = await _userManager.ChangePasswordAsync(
            user,
            request.CurrentPassword,
            request.NewPassword);

        if (!result.Succeeded)
            throw new InvalidOperationException(
                string.Join(", ", result.Errors.Select(x => x.Description)));

        await _tokens.RevokeAllByUserIdAsync(userId);
    }

    public async Task DeactivateAccountAsync(int userId)
    {
        var user = await _users.FindByIdAsync(userId)
                   ?? throw new KeyNotFoundException("User not found.");

        user.IsActive = false;
        await _users.UpdateAsync(user);
        await _tokens.RevokeAllByUserIdAsync(userId);
    }

    public async Task ReactivateAccountAsync(int userId)
    {
        var user = await _users.FindByIdAsync(userId)
                   ?? throw new KeyNotFoundException("User not found.");

        user.IsActive = true;
        await _users.UpdateAsync(user);
    }

    public async Task DeleteAccountAsync(int userId)
    {
        await _users.DeleteAsync(userId);
    }

    public bool ValidateToken(string token)
        => _jwt.ValidateToken(token);

    public Task<int?> GetUserIdFromTokenAsync(string token)
        => Task.FromResult(_jwt.GetUserId(token));

    private async Task<AuthResponse> BuildAuthResponse(User user)
    {
        var accessToken = _jwt.GenerateAccessToken(user);
        var refreshToken = _jwt.GenerateRefreshToken();

        await _tokens.AddAsync(new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            ExpiresAt = _jwt.GetRefreshTokenExpiry(),
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow
        });

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = _jwt.GetAccessTokenExpiry(),
            User = Map(user)
        };
    }

    private static UserResponse Map(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email ?? "",
            PhoneNumber = user.PhoneNumber ?? "",
            Role = user.Role,
            VehiclePlate = user.VehiclePlate,
            ProfilePicUrl = user.ProfilePicUrl,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        };
    }
}