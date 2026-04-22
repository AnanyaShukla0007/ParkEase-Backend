using Microsoft.EntityFrameworkCore;
using Payment.Application.Interfaces;
using Payment.Infrastructure.Persistence;
using PaymentEntity = Payment.Domain.Entities.Payment;

namespace Payment.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly PaymentDbContext _context;

    public PaymentRepository(PaymentDbContext context)
    {
        _context = context;
    }

    public async Task<PaymentEntity> AddAsync(PaymentEntity payment)
    {
        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<PaymentEntity?> GetByIdAsync(int id)
    {
        return await _context.Payments.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<PaymentEntity>> GetByBookingIdAsync(int bookingId)
    {
        return await _context.Payments
            .Where(x => x.BookingId == bookingId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync();
    }

    public async Task<List<PaymentEntity>> GetByUserIdAsync(int userId)
    {
        return await _context.Payments
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync();
    }

    public async Task<List<PaymentEntity>> GetAllAsync()
    {
        return await _context.Payments
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync();
    }

    public async Task UpdateAsync(PaymentEntity payment)
    {
        _context.Payments.Update(payment);
        await _context.SaveChangesAsync();
    }
}
