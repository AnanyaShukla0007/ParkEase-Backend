using Vehicle.Domain.Enums;

namespace Vehicle.Application.DTOs;

public class UpdateVehicleRequest
{
    public string? LicensePlate { get; set; }
    public string? Make { get; set; }
    public string? Model { get; set; }
    public string? Color { get; set; }
    public VehicleType? VehicleType { get; set; }
    public bool? IsEV { get; set; }
    public bool? IsActive { get; set; }
}
