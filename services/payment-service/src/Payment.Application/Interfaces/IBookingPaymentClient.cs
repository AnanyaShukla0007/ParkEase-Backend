using Payment.Application.DTOs;

namespace Payment.Application.Interfaces;

public interface IBookingPaymentClient
{
    Task<BookingLookupResponse?> GetBookingAsync(int bookingId);
}
