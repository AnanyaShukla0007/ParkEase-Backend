namespace Auth.Application.DTOs;

public class GoogleAuthRequest
{
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string GoogleId { get; set; } = string.Empty;
    public string Role { get; set; } = "DRIVER";
}