namespace Analytics.Application.DTOs;

public class AnalyticsPaymentSnapshot
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public int Status { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? PaidAtUtc { get; set; }
}
