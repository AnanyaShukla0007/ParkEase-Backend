namespace Payment.Application.DTOs;

public class ProcessPaymentRequest
{
    public string TransactionReference { get; set; } = string.Empty;
    public string? ProviderReference { get; set; }
    public string? Notes { get; set; }
}
