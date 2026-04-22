using PaymentEntity = Payment.Domain.Entities.Payment;

namespace Payment.Application.Interfaces;

public interface IPaymentRepository
{
    Task<PaymentEntity> AddAsync(PaymentEntity payment);
    Task<PaymentEntity?> GetByIdAsync(int id);
    Task<List<PaymentEntity>> GetByBookingIdAsync(int bookingId);
    Task<List<PaymentEntity>> GetByUserIdAsync(int userId);
    Task<List<PaymentEntity>> GetAllAsync();
    Task UpdateAsync(PaymentEntity payment);
}
