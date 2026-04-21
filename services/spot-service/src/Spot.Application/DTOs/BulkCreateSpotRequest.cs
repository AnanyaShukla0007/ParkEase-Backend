using ParkEase.Spot.Domain.Enums;

namespace ParkEase.Spot.Application.DTOs
{
    public class BulkCreateSpotRequest
    {
        public int LotId { get; set; }
        public int Count { get; set; }
        public int StartNumber { get; set; } = 1;
        public int Floor { get; set; }
        public SpotType SpotType { get; set; }
        public VehicleType VehicleType { get; set; }
        public bool IsHandicapped { get; set; }
        public bool IsEVCharging { get; set; }
        public decimal PricePerHour { get; set; }
    }
}