namespace Analytics.Application.Interfaces;

public interface ISpotAnalyticsClient
{
    Task<string?> GetSpotTypeAsync(int spotId);
}
