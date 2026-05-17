using Payment.Application.DTOs;
using Payment.Application.Interfaces;
using Payment.Application.Validators;
using Payment.Domain.Enums;
using PaymentEntity = Payment.Domain.Entities.Payment;

namespace Payment.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IBookingPaymentClient _bookingClient;

    public PaymentService(
        IPaymentRepository paymentRepository,
        IBookingPaymentClient bookingClient)
    {
        _paymentRepository = paymentRepository;
        _bookingClient = bookingClient;
    }

    public async Task<PaymentResponse> CreateAsync(CreatePaymentRequest request)
    {
        var errors = CreatePaymentValidator.Validate(request);

        if (errors.Count > 0)
            throw new InvalidOperationException(string.Join(" | ", errors));

        var payment = new PaymentEntity
        {
            BookingId = request.BookingId,
            UserId = request.UserId,
            Amount = request.Amount,
            Currency = request.Currency.Trim().ToUpperInvariant(),
            PaymentMethod = request.PaymentMethod,
            Status = PaymentStatus.Pending,
            Notes = request.Notes,
            CreatedAtUtc = DateTime.UtcNow
        };

        var saved = await _paymentRepository.AddAsync(payment);
        return Map(saved);
    }

    public async Task<PaymentResponse?> GetByIdAsync(int id)
    {
        var payment = await _paymentRepository.GetByIdAsync(id);
        return payment is null ? null : Map(payment);
    }

    public async Task<List<PaymentResponse>> GetByBookingIdAsync(int bookingId)
        => (await _paymentRepository.GetByBookingIdAsync(bookingId)).Select(Map).ToList();

    public async Task<List<PaymentResponse>> GetByUserIdAsync(int userId)
        => (await _paymentRepository.GetByUserIdAsync(userId)).Select(Map).ToList();

    public async Task<List<PaymentResponse>> GetAllAsync()
        => (await _paymentRepository.GetAllAsync()).Select(Map).ToList();

    public async Task<PaymentResponse> ProcessAsync(int id, ProcessPaymentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.TransactionReference))
            throw new InvalidOperationException("TransactionReference is required.");

        var payment = await _paymentRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Payment not found.");

        if (payment.Status == PaymentStatus.Paid)
            throw new InvalidOperationException("Payment is already processed.");

        if (payment.Status == PaymentStatus.Refunded)
            throw new InvalidOperationException("Refunded payment cannot be processed again.");

        payment.Status = PaymentStatus.Paid;
        payment.TransactionReference = request.TransactionReference.Trim();
        payment.ProviderReference = string.IsNullOrWhiteSpace(request.ProviderReference)
            ? payment.ProviderReference
            : request.ProviderReference.Trim();
        payment.Notes = string.IsNullOrWhiteSpace(request.Notes)
            ? payment.Notes
            : request.Notes.Trim();
        payment.PaidAtUtc = DateTime.UtcNow;
        payment.UpdatedAtUtc = DateTime.UtcNow;

        await _paymentRepository.UpdateAsync(payment);
        return Map(payment);
    }

    public async Task<PaymentResponse> FailAsync(int id, FailPaymentRequest request)
    {
        var payment = await _paymentRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Payment not found.");

        if (payment.Status == PaymentStatus.Paid)
            throw new InvalidOperationException("Paid payment cannot be marked as failed.");

        if (payment.Status == PaymentStatus.Refunded)
            throw new InvalidOperationException("Refunded payment cannot be marked as failed.");

        payment.Status = PaymentStatus.Failed;
        payment.Notes = string.IsNullOrWhiteSpace(request.Reason)
            ? payment.Notes
            : request.Reason.Trim();
        payment.UpdatedAtUtc = DateTime.UtcNow;

        await _paymentRepository.UpdateAsync(payment);
        return Map(payment);
    }

    public async Task<PaymentResponse> RefundAsync(int id, RefundPaymentRequest request)
    {
        var payment = await _paymentRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Payment not found.");

        if (payment.Status != PaymentStatus.Paid)
            throw new InvalidOperationException("Only paid payments can be refunded.");

        var booking = await _bookingClient.GetBookingAsync(payment.BookingId)
            ?? throw new InvalidOperationException("Booking details are required before refund.");

        var refundAmount = GetRefundAmount(payment, booking);

        payment.Status = PaymentStatus.Refunded;
        payment.RefundedAtUtc = DateTime.UtcNow;
        payment.UpdatedAtUtc = DateTime.UtcNow;
        payment.Notes = $"Refund amount: {refundAmount:0.##} {payment.Currency}. " +
                        (string.IsNullOrWhiteSpace(request.Reason)
                            ? "Eligible refund processed."
                            : request.Reason.Trim());

        await _paymentRepository.UpdateAsync(payment);
        return Map(payment);
    }

    private static decimal GetRefundAmount(PaymentEntity payment, BookingLookupResponse booking)
    {
        if (booking.IsCancelled && booking.CheckInTimeUtc is null)
        {
            var cancelledAt = booking.UpdatedAtUtc ?? DateTime.UtcNow;
            var minutesFromReservation = (cancelledAt - booking.CreatedAtUtc).TotalMinutes;

            if (minutesFromReservation <= 15)
                return payment.Amount;

            throw new InvalidOperationException(
                "Refund is only available when a reservation is cancelled within 15 minutes before check-in.");
        }

        if (!booking.IsCompleted || booking.CheckInTimeUtc is null || booking.CheckOutTimeUtc is null)
            throw new InvalidOperationException("Refund is only available after checkout or eligible cancellation.");

        var parkedMinutes = (booking.CheckOutTimeUtc.Value - booking.CheckInTimeUtc.Value).TotalMinutes;

        if (parkedMinutes < 30)
            return decimal.Round(payment.Amount / 2, 2);

        throw new InvalidOperationException("Refund is only available if you parked for less than 30 minutes.");
    }

    private static PaymentResponse Map(PaymentEntity payment) => new()
    {
        Id = payment.Id,
        BookingId = payment.BookingId,
        UserId = payment.UserId,
        Amount = payment.Amount,
        Currency = payment.Currency,
        PaymentMethod = payment.PaymentMethod,
        Status = payment.Status,
        TransactionReference = payment.TransactionReference,
        ProviderReference = payment.ProviderReference,
        Notes = payment.Notes,
        PaidAtUtc = payment.PaidAtUtc,
        RefundedAtUtc = payment.RefundedAtUtc,
        CreatedAtUtc = payment.CreatedAtUtc,
        UpdatedAtUtc = payment.UpdatedAtUtc
    };
}
