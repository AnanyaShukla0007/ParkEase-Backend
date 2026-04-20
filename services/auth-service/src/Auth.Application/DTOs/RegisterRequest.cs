using System.ComponentModel.DataAnnotations;

namespace Auth.Application.DTOs;

public class RegisterRequest
{
    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = "DRIVER";

    public string? VehiclePlate { get; set; }
}