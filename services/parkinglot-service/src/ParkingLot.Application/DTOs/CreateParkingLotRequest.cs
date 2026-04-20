using System.ComponentModel.DataAnnotations;

namespace ParkingLot.Application.DTOs;

public class CreateParkingLotRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Address { get; set; } = string.Empty;

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string State { get; set; } = string.Empty;

    [Required]
    public string Pincode { get; set; } = string.Empty;

    [Range(-90, 90)]
    public double Latitude { get; set; }

    [Range(-180, 180)]
    public double Longitude { get; set; }

    [Range(1, int.MaxValue)]
    public int TotalSpots { get; set; }

    [Required]
    public int ManagerId { get; set; }

    public TimeSpan OpenTime { get; set; } = new(0, 0, 0);

    public TimeSpan CloseTime { get; set; } = new(23, 59, 0);

    public string? ImageUrl { get; set; }
}