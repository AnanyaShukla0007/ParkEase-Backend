using Payment.Application.DTOs;

namespace Payment.Application.Interfaces;

public interface IPaymentService
{
    Task<PaymentResponse> CreateAsync(CreatePaymentRequest request);
    Task<PaymentResponse?> GetByIdAsync(int id);
    Task<List<PaymentResponse>> GetByBookingIdAsync(int bookingId);
    Task<List<PaymentResponse>> GetByUserIdAsync(int userId);
    Task<List<PaymentResponse>> GetAllAsync();
    Task<PaymentResponse> ProcessAsync(int id, ProcessPaymentRequest request);
    Task<PaymentResponse> FailAsync(int id, FailPaymentRequest request);
    Task<PaymentResponse> RefundAsync(int id, RefundPaymentRequest request);
}
