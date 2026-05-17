using System.Text.Json.Serialization;

namespace Payment.Application.DTOs;

public class BookingLookupEnvelope
{
    public bool Success { get; set; }
    public BookingLookupResponse? Data { get; set; }
}

public class BookingLookupResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int Status { get; set; }
    public DateTime StartTimeUtc { get; set; }
    public DateTime EndTimeUtc { get; set; }
    public DateTime? CheckInTimeUtc { get; set; }
    public DateTime? CheckOutTimeUtc { get; set; }
    public decimal EstimatedAmount { get; set; }
    public decimal FinalAmount { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }

    [JsonIgnore]
    public bool IsCancelled => Status == 5;

    [JsonIgnore]
    public bool IsCompleted => Status == 4;
}
