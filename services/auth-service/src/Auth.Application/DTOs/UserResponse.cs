namespace Auth.Application.DTOs;

public class UserResponse
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string TemporaryPassword { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string? VehiclePlate { get; set; }

    public string? ProfilePicUrl { get; set; }

    public string ManagerApplicationStatus { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string ApplicationStatus { get; set; } = string.Empty;

    public string? ManagerApplicationNotes { get; set; }

    public string? ProposedLotName { get; set; }

    public string? ProposedLotAddress { get; set; }

    public string? ProposedLotCity { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}
