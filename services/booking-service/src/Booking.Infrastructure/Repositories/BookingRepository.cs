using Booking.Application.Interfaces;
using Booking.Domain.Enums;
using Booking.Infrastructure.Persistence;
using BookingEntity = Booking.Domain.Entities.Booking;
using Microsoft.EntityFrameworkCore;

namespace Booking.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly BookingDbContext _context;

    public BookingRepository(BookingDbContext context)
    {
        _context = context;
    }

    public async Task<BookingEntity?> GetByIdAsync(int id)
    {
        return await _context.Bookings.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<BookingEntity>> GetByUserIdAsync(int userId)
    {
        return await _context.Bookings
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync();
    }

    public async Task<List<BookingEntity>> GetByLotIdAsync(int lotId)
    {
        return await _context.Bookings
            .Where(x => x.LotId == lotId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync();
    }

    public async Task<List<BookingEntity>> GetAllAsync()
    {
        return await _context.Bookings
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync();
    }

    public async Task<bool> HasOverlappingBookingAsync(
        int spotId,
        DateTime startUtc,
        DateTime endUtc)
    {
        return await _context.Bookings.AnyAsync(x =>
            x.SpotId == spotId &&
            x.Status != BookingStatus.Cancelled &&
            x.Status != BookingStatus.Completed &&
            x.Status != BookingStatus.Expired &&
            startUtc < x.EndTimeUtc &&
            endUtc > x.StartTimeUtc);
    }

    public async Task AddAsync(BookingEntity booking)
    {
        await _context.Bookings.AddAsync(booking);
    }

    public void Update(BookingEntity booking)
    {
        _context.Bookings.Update(booking);
    }

    public void Delete(BookingEntity booking)
    {
        _context.Bookings.Remove(booking);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}