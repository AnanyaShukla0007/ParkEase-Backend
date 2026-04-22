using Booking.Application.DTOs.Requests;

namespace Booking.Application.Validators;

public static class ExtendBookingValidator
{
    public static List<string> Validate(ExtendBookingRequest request)
    {
        var errors = new List<string>();

        if (request.NewEndTimeUtc == default)
            errors.Add("NewEndTimeUtc is required.");

        return errors;
    }
}