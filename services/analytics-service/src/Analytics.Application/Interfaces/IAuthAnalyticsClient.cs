namespace Analytics.Application.Interfaces;

public interface IAuthAnalyticsClient
{
    Task<bool> UserExistsAsync(int userId);
}
