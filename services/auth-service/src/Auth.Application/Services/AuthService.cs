using Auth.Application.DTOs;
using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

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
        var email = request.Email.Trim().ToLowerInvariant();
        var phone = request.PhoneNumber.Trim();
        var role = request.Role.Trim().ToUpperInvariant();

        if (role is not "DRIVER" and not "MANAGER" and not "ADMIN")
            role = "DRIVER";

        var existingUser = await _users.FindByEmailAsync(email);
        if (existingUser is not null)
        {
            if (role == "MANAGER" && existingUser.Role is "DRIVER" or "MANAGER")
            {
                var phoneOwner = await _users.FindByPhoneAsync(phone);
                if (phoneOwner is not null && phoneOwner.Id != existingUser.Id)
                    throw new InvalidOperationException("Phone already registered.");

                existingUser.FullName = request.FullName.Trim();
                existingUser.PhoneNumber = phone;
                existingUser.Role = "MANAGER";
                existingUser.IsActive = true;
                existingUser.UpdatedAt = DateTime.UtcNow;

                await _users.UpdateAsync(existingUser);

                if (!await _userManager.IsInRoleAsync(existingUser, "MANAGER"))
                    await _userManager.AddToRoleAsync(existingUser, "MANAGER");

                return await BuildAuthResponse(existingUser);
            }

            throw new InvalidOperationException("Email already registered.");
        }

        if (await _users.ExistsByPhoneAsync(phone))
            throw new InvalidOperationException("Phone already registered.");

        var user = new User
        {
            FullName = request.FullName.Trim(),
            Email = email,
            UserName = email,
            PhoneNumber = phone,
            Role = role,
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

    public Task<AuthResponse> ApplyManagerAsync(ManagerApplicationRequest request)
    {
        var fullName = FirstValue(
            request.FullName,
            request.Name,
            ExtraValue(request, "fullName", "name", "applicantName", "ownerName"));
        var phone = FirstValue(
            request.PhoneNumber,
            request.Phone,
            ExtraValue(request, "phoneNumber", "phone", "mobile", "mobileNumber", "contactNumber"));
        var lotName = FirstValue(
            request.ProposedLotName,
            request.LotName,
            ExtraValue(request, "proposedLotName", "lotName", "parkingLotName", "facilityName"));
        var address = FirstValue(
            request.Address,
            request.LotAddress,
            request.FacilityAddress,
            request.Location,
            ExtraValue(request, "address", "lotAddress", "facilityAddress", "parkingAddress", "location", "addressLine", "street"));
        var city = FirstValue(
            request.City,
            request.LotCity,
            request.FacilityCity,
            ExtraValue(request, "city", "lotCity", "facilityCity", "parkingCity"));

        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name is required.");

        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ArgumentException("Email is required.");

        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone number is required.");

        if (string.IsNullOrWhiteSpace(lotName))
            throw new ArgumentException("Proposed lot name is required.");

        return RegisterAsync(new RegisterRequest
        {
            FullName = fullName,
            Email = request.Email,
            PhoneNumber = phone,
            Password = "Manager@ParkEase2026",
            Role = "MANAGER"
        });
    }


    public async Task<AuthResponse> GoogleAuthAsync(GoogleAuthRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.FullName) ||
            string.IsNullOrWhiteSpace(request.GoogleId))
        {
            throw new InvalidOperationException("Google account details are incomplete.");
        }

        var email = request.Email.Trim().ToLowerInvariant();
        var user = await _users.FindByEmailAsync(email);

        if (user is null)
        {
            var role = string.IsNullOrWhiteSpace(request.Role)
                ? "DRIVER"
                : request.Role.Trim().ToUpperInvariant();

            if (role is not "DRIVER" and not "MANAGER")
                role = "DRIVER";

            user = new User
            {
                FullName = request.FullName.Trim(),
                Email = email,
                UserName = email,
                Role = role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ", result.Errors.Select(x => x.Description)));
            }

            await _userManager.AddToRoleAsync(user, user.Role);
        }

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Account disabled.");

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

    public async Task<bool> UserExistsAsync(int userId)
        => await _users.FindByIdAsync(userId) is not null;

    public async Task<int> GetUserCountAsync()
        => (await _users.GetAllAsync()).Count;

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

    private static string FirstValue(params string?[] values)
        => values.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x))?.Trim() ?? string.Empty;

    private static string ExtraValue(
        ManagerApplicationRequest request,
        params string[] keys)
    {
        foreach (var key in keys)
        {
            var match = request.ExtraFields.FirstOrDefault(x =>
                NormalizeKey(x.Key) == NormalizeKey(key));

            if (!string.IsNullOrWhiteSpace(match.Key) &&
                TryGetString(match.Value, out var value))
            {
                return value;
            }
        }

        return string.Empty;
    }

    private static string NormalizeKey(string key)
        => new(key.Where(char.IsLetterOrDigit).Select(char.ToLowerInvariant).ToArray());

    private static bool TryGetString(JsonElement element, out string value)
    {
        value = string.Empty;

        if (element.ValueKind == JsonValueKind.String)
        {
            value = element.GetString() ?? string.Empty;
            return !string.IsNullOrWhiteSpace(value);
        }

        if (element.ValueKind == JsonValueKind.Number ||
            element.ValueKind == JsonValueKind.True ||
            element.ValueKind == JsonValueKind.False)
        {
            value = element.ToString();
            return !string.IsNullOrWhiteSpace(value);
        }

        return false;
    }
}
