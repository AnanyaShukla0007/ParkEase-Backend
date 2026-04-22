namespace Analytics.Application.DTOs;

public class TrackDemandSignalRequest
{
    public int? UserId { get; set; }
    public string City { get; set; } = string.Empty;
    public int? LotId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string? SearchTerm { get; set; }
    public string? Reason { get; set; }
    public DateTime? OccurredAt { get; set; }
}
