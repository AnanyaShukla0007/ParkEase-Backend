namespace Analytics.Application.DTOs;

public class FailedHourResponse
{
    public int HourOfDay { get; set; }
    public int FailedEventCount { get; set; }
}
