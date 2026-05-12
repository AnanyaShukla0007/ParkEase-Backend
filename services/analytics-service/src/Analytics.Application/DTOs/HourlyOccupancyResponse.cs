namespace Analytics.Application.DTOs;

public class HourlyOccupancyResponse
{
    public int HourOfDay { get; set; }
    public decimal AverageOccupancyRate { get; set; }
}
