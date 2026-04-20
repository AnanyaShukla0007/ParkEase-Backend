using System.ComponentModel.DataAnnotations;

namespace Auth.Application.DTOs;

public class UpdateProfileRequest
{
    [StringLength(100)]
    public string? FullName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? VehiclePlate { get; set; }

    public string? ProfilePicUrl { get; set; }
}