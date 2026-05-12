namespace Analytics.Application.DTOs;

public class LogOccupancyRequest
{
    public int LotId { get; set; }
    public int SpotId { get; set; }
    public DateTime Timestamp { get; set; }
    public decimal OccupancyRate { get; set; }
    public int AvailableSpots { get; set; }
    public int TotalSpots { get; set; }
    public string VehicleType { get; set; } = string.Empty;
}
