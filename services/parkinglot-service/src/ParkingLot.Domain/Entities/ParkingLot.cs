namespace ParkingLot.Domain.Entities;

public class ParkingLot
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public string Pincode { get; set; } = string.Empty;

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public int TotalSpots { get; set; }

    public int AvailableSpots { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsApproved { get; set; } = false;

    public bool IsOpen { get; set; } = true;

    public int ManagerId { get; set; }

    public TimeSpan OpenTime { get; set; } = new(0, 0, 0);

    public TimeSpan CloseTime { get; set; } = new(23, 59, 0);

    public string? ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}