using Booking.Application.DTOs.Requests;

namespace Booking.Application.Validators;

public static class CreateBookingValidator
{
    public static List<string> Validate(CreateBookingRequest request)
    {
        var errors = new List<string>();

        if (request.UserId <= 0)
            errors.Add("Valid UserId is required.");

        if (request.LotId <= 0)
            errors.Add("Valid LotId is required.");

        if (request.SpotId <= 0)
            errors.Add("Valid SpotId is required.");

        if (request.VehicleId <= 0)
            errors.Add("Valid VehicleId is required.");

        if (string.IsNullOrWhiteSpace(request.VehiclePlate))
            errors.Add("VehiclePlate is required.");

        if (request.EndTimeUtc <= request.StartTimeUtc)
            errors.Add("End time must be greater than start time.");

        if (request.EstimatedAmount < 0)
            errors.Add("EstimatedAmount cannot be negative.");

        return errors;
    }
}