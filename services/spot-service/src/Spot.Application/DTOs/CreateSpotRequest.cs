using ParkEase.Spot.Domain.Enums;

namespace ParkEase.Spot.Application.DTOs
{
    public class CreateSpotRequest
    {
        public int LotId { get; set; }
        public string SpotNumber { get; set; } = string.Empty;
        public int Floor { get; set; }
        public SpotType SpotType { get; set; }
        public VehicleType VehicleType { get; set; }
        public bool IsHandicapped { get; set; }
        public bool IsEVCharging { get; set; }
        public decimal PricePerHour { get; set; }
    }
}