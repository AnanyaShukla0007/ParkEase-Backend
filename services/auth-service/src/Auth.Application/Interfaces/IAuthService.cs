using Auth.Application.DTOs;

namespace Auth.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);

    Task<AuthResponse> LoginAsync(LoginRequest request);

    Task<AuthResponse> RefreshTokenAsync(string refreshToken);

    Task LogoutAsync(int userId, string refreshToken);

    Task<UserResponse> GetUserByIdAsync(int userId);

    Task<bool> UserExistsAsync(int userId);

    Task<int> GetUserCountAsync();

    Task<List<UserResponse>> GetAllUsersAsync();

    Task<List<UserResponse>> GetUsersByRoleAsync(string role);

    Task<UserResponse> UpdateProfileAsync(
        int userId,
        UpdateProfileRequest request);

    Task ChangePasswordAsync(
        int userId,
        ChangePasswordRequest request);

    Task DeactivateAccountAsync(int userId);

    Task ReactivateAccountAsync(int userId);

    Task DeleteAccountAsync(int userId);

    bool ValidateToken(string token);

    Task<int?> GetUserIdFromTokenAsync(string token);
}
