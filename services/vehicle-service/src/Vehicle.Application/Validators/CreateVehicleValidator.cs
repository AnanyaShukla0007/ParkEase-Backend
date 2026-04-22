using Vehicle.Application.DTOs;

namespace Vehicle.Application.Validators;

public static class CreateVehicleValidator
{
    public static List<string> Validate(CreateVehicleRequest request)
    {
        var errors = new List<string>();

        if (request.OwnerId <= 0)
            errors.Add("Valid OwnerId is required.");

        if (string.IsNullOrWhiteSpace(request.LicensePlate))
            errors.Add("LicensePlate is required.");

        if (string.IsNullOrWhiteSpace(request.Make))
            errors.Add("Make is required.");

        if (string.IsNullOrWhiteSpace(request.Model))
            errors.Add("Model is required.");

        if (string.IsNullOrWhiteSpace(request.Color))
            errors.Add("Color is required.");

        return errors;
    }
}
