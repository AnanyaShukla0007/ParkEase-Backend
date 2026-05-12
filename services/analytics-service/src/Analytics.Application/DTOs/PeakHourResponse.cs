namespace Analytics.Application.DTOs;

public class PeakHourResponse
{
    public int HourOfDay { get; set; }
    public decimal AverageOccupancyRate { get; set; }
}
