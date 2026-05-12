namespace Analytics.Application.DTOs;

public class UserTrustScoreResponse
{
    public int UserId { get; set; }
    public int Score { get; set; }
    public string Tier { get; set; } = string.Empty;
    public int CancellationCount { get; set; }
    public int NoShowCount { get; set; }
    public int OverstayCount { get; set; }
    public int OnTimeExitCount { get; set; }
    public int SuccessfulPaymentCount { get; set; }
    public DateTime CalculatedAt { get; set; }
}
