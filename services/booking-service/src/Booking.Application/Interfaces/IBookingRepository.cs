using BookingEntity = Booking.Domain.Entities.Booking;

namespace Booking.Application.Interfaces;

public interface IBookingRepository
{
    Task<BookingEntity?> GetByIdAsync(int id);
    Task<List<BookingEntity>> GetByUserIdAsync(int userId);
    Task<List<BookingEntity>> GetByLotIdAsync(int lotId);
    Task<List<BookingEntity>> GetAllAsync();

    Task<bool> HasOverlappingBookingAsync(
        int spotId,
        DateTime startUtc,
        DateTime endUtc);

    Task AddAsync(BookingEntity booking);
    void Update(BookingEntity booking);
    void Delete(BookingEntity booking);

    Task<int> SaveChangesAsync();
}