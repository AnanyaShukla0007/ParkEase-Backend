using ParkEase.Spot.Domain.Enums;

namespace ParkEase.Spot.Domain.Entities
{
    public class Spot
    {
        public int SpotId { get; set; }

        public int LotId { get; set; }

        public string SpotNumber { get; set; } = string.Empty;

        public int Floor { get; set; }

        public SpotType SpotType { get; set; }

        public VehicleType VehicleType { get; set; }

        public SpotStatus Status { get; set; } = SpotStatus.Available;

        public bool IsHandicapped { get; set; }

        public bool IsEVCharging { get; set; }

        public decimal PricePerHour { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}