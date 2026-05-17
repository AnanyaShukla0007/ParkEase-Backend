using Microsoft.AspNetCore.Identity;

namespace Auth.Domain.Entities;

public class User : IdentityUser<int>
{
    public string FullName { get; set; } = string.Empty;

    public string Role { get; set; } = "DRIVER";

    public string? VehiclePlate { get; set; }

    public string? ProfilePicUrl { get; set; }

    public string ManagerApplicationStatus { get; set; } = "APPROVED";

    public string? ManagerApplicationNotes { get; set; }

    public string? ProposedLotName { get; set; }

    public string? ProposedLotAddress { get; set; }

    public string? ProposedLotCity { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public ICollection<RefreshToken> RefreshTokens { get; set; }
        = new List<RefreshToken>();
}
