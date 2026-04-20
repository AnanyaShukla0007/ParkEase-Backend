namespace ParkingLot.Application.DTOs;

public class ParkingLotResponse
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

    public bool IsActive { get; set; }

    public bool IsApproved { get; set; }

    public bool IsOpen { get; set; }

    public int ManagerId { get; set; }

    public TimeSpan OpenTime { get; set; }

    public TimeSpan CloseTime { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }
}