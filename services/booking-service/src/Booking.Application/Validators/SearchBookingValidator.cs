using Booking.Application.DTOs.Requests;

namespace Booking.Application.Validators;

public static class SearchBookingValidator
{
    public static List<string> Validate(SearchBookingRequest request)
    {
        var errors = new List<string>();

        if (request.FromUtc.HasValue &&
            request.ToUtc.HasValue &&
            request.ToUtc < request.FromUtc)
        {
            errors.Add("ToUtc cannot be earlier than FromUtc.");
        }

        return errors;
    }
}