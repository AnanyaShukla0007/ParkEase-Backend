using System.Security.Claims;
using Auth.Application.DTOs;
using Auth.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers;

[ApiController]
[Route("api/v1/auth")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    /// <summary>Register a new account</summary>
    /// <remarks>
    /// Creates a new DRIVER or MANAGER account and returns access token,
    /// refresh token, expiry time, and user profile.
    /// </remarks>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        => Ok(await _service.RegisterAsync(request));

    /// <summary>Login user</summary>
    /// <remarks>
    /// Validates email and password, then returns JWT access token,
    /// refresh token, and user details.
    /// </remarks>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
        => Ok(await _service.LoginAsync(request));

    /// <summary>Refresh expired access token</summary>
    /// <remarks>
    /// Accepts a valid refresh token and returns a new access token pair.
    /// </remarks>
    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        => Ok(await _service.RefreshTokenAsync(request.RefreshToken));

    /// <summary>Logout current user</summary>
    /// <remarks>
    /// Revokes the provided refresh token and logs the user out.
    /// </remarks>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request)
    {
        await _service.LogoutAsync(GetUserId(), request.RefreshToken);
        return Ok(new { message = "Logged out successfully." });
    }

    /// <summary>Get current user profile</summary>
    /// <remarks>
    /// Returns profile details of the authenticated user.
    /// </remarks>
    [HttpGet("profile")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Profile()
        => Ok(await _service.GetUserByIdAsync(GetUserId()));

    /// <summary>Update current user profile</summary>
    /// <remarks>
    /// Updates name, phone number, vehicle plate, or profile picture.
    /// </remarks>
    [HttpPut("profile")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromBody] UpdateProfileRequest request)
        => Ok(await _service.UpdateProfileAsync(GetUserId(), request));

    /// <summary>Change account password</summary>
    /// <remarks>
    /// Changes password after validating current password.
    /// Existing refresh tokens may be revoked.
    /// </remarks>
    [HttpPut("password")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Password([FromBody] ChangePasswordRequest request)
    {
        await _service.ChangePasswordAsync(GetUserId(), request);
        return Ok(new { message = "Password changed successfully." });
    }

    /// <summary>Deactivate current account</summary>
    /// <remarks>
    /// Disables the current user's account and blocks future login.
    /// </remarks>
    [HttpDelete("deactivate")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Deactivate()
    {
        await _service.DeactivateAccountAsync(GetUserId());
        return Ok(new { message = "Account deactivated." });
    }

    /// <summary>Validate JWT token</summary>
    /// <remarks>
    /// Used by gateway or other microservices to verify token validity.
    /// </remarks>
    [HttpGet("validate")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Validate([FromQuery] string token)
        => Ok(new { isValid = _service.ValidateToken(token) });

    /// <summary>Get all users</summary>
    /// <remarks>
    /// ADMIN only endpoint. Returns all registered users.
    /// </remarks>
    [HttpGet("users")]
    [Authorize(Roles = "ADMIN")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsers()
        => Ok(await _service.GetAllUsersAsync());

    /// <summary>Get users by role</summary>
    /// <remarks>
    /// ADMIN only endpoint. Returns users filtered by role:
    /// DRIVER, MANAGER, or ADMIN.
    /// </remarks>
    [HttpGet("users/role/{role}")]
    [Authorize(Roles = "ADMIN")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsersByRole(string role)
        => Ok(await _service.GetUsersByRoleAsync(role));

    /// <summary>Get user by ID</summary>
    /// <remarks>
    /// ADMIN only endpoint. Returns a specific user profile by user ID.
    /// </remarks>
    [HttpGet("users/{userId:int}")]
    [Authorize(Roles = "ADMIN")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(int userId)
        => Ok(await _service.GetUserByIdAsync(userId));

    /// <summary>Suspend user account</summary>
    /// <remarks>
    /// ADMIN only endpoint. Deactivates selected user account.
    /// </remarks>
    [HttpPut("users/{userId:int}/suspend")]
    [Authorize(Roles = "ADMIN")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SuspendUser(int userId)
    {
        await _service.DeactivateAccountAsync(userId);
        return Ok(new { message = $"User {userId} suspended." });
    }

    /// <summary>Reactivate user account</summary>
    /// <remarks>
    /// ADMIN only endpoint. Restores access for suspended user.
    /// </remarks>
    [HttpPut("users/{userId:int}/reactivate")]
    [Authorize(Roles = "ADMIN")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ReactivateUser(int userId)
    {
        await _service.ReactivateAccountAsync(userId);
        return Ok(new { message = $"User {userId} reactivated." });
    }

    /// <summary>Delete user permanently</summary>
    /// <remarks>
    /// ADMIN only endpoint. Permanently removes user account.
    /// </remarks>
    [HttpDelete("users/{userId:int}")]
    [Authorize(Roles = "ADMIN")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        await _service.DeleteAccountAsync(userId);
        return Ok(new { message = $"User {userId} deleted." });
    }

    private int GetUserId()
    {
        var value =
            User.FindFirst("userId")?.Value ??
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return int.Parse(value!);
    }
}