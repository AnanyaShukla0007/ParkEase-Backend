using Analytics.Application.DTOs;

namespace Analytics.Application.Validators;

public static class AnalyticsValidator
{
    public static List<string> Validate(LogOccupancyRequest request)
    {
        var errors = new List<string>();

        if (request.LotId <= 0)
            errors.Add("Valid LotId is required.");
        if (request.SpotId <= 0)
            errors.Add("Valid SpotId is required.");
        if (request.TotalSpots <= 0)
            errors.Add("TotalSpots must be greater than zero.");
        if (request.AvailableSpots < 0)
            errors.Add("AvailableSpots cannot be negative.");
        if (request.OccupancyRate < 0 || request.OccupancyRate > 100)
            errors.Add("OccupancyRate must be between 0 and 100.");
        if (string.IsNullOrWhiteSpace(request.VehicleType))
            errors.Add("VehicleType is required.");

        return errors;
    }

    public static List<string> Validate(TrackDemandSignalRequest request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.City))
            errors.Add("City is required.");
        if (string.IsNullOrWhiteSpace(request.EventType))
            errors.Add("EventType is required.");

        return errors;
    }
}
