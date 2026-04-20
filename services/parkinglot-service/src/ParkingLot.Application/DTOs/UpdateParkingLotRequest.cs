namespace ParkingLot.Application.DTOs;

public class UpdateParkingLotRequest
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Pincode { get; set; }

    public int? TotalSpots { get; set; }

    public bool? IsOpen { get; set; }

    public string? ImageUrl { get; set; }

    public TimeSpan? OpenTime { get; set; }

    public TimeSpan? CloseTime { get; set; }
}