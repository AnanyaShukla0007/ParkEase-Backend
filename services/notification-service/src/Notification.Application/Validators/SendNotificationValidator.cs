using Notification.Application.DTOs;

namespace Notification.Application.Validators;

public static class SendNotificationValidator
{
    public static List<string> Validate(SendNotificationRequest request)
    {
        var errors = new List<string>();

        if (request.RecipientId <= 0)
            errors.Add("Valid RecipientId is required.");

        if (string.IsNullOrWhiteSpace(request.Title))
            errors.Add("Title is required.");

        if (string.IsNullOrWhiteSpace(request.Message))
            errors.Add("Message is required.");

        return errors;
    }

    public static List<string> Validate(SendBulkNotificationsRequest request)
    {
        var errors = new List<string>();

        if (request.RecipientIds.Count == 0)
            errors.Add("At least one RecipientId is required.");

        if (string.IsNullOrWhiteSpace(request.Title))
            errors.Add("Title is required.");

        if (string.IsNullOrWhiteSpace(request.Message))
            errors.Add("Message is required.");

        return errors;
    }
}
