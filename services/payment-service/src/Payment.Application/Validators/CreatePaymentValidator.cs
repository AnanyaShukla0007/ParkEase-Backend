using Payment.Application.DTOs;

namespace Payment.Application.Validators;

public static class CreatePaymentValidator
{
    public static List<string> Validate(CreatePaymentRequest request)
    {
        var errors = new List<string>();

        if (request.BookingId <= 0)
            errors.Add("Valid BookingId is required.");

        if (request.UserId <= 0)
            errors.Add("Valid UserId is required.");

        if (request.Amount <= 0)
            errors.Add("Amount must be greater than zero.");

        if (string.IsNullOrWhiteSpace(request.Currency))
            errors.Add("Currency is required.");

        return errors;
    }
}
