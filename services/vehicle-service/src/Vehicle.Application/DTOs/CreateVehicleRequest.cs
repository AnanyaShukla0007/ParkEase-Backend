using Vehicle.Domain.Enums;

namespace Vehicle.Application.DTOs;

public class CreateVehicleRequest
{
    public int OwnerId { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public VehicleType VehicleType { get; set; }
    public bool IsEV { get; set; }
    public bool IsActive { get; set; } = true;
}
