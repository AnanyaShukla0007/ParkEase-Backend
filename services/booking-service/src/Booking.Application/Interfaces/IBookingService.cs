using Booking.Application.DTOs.Requests;
using Booking.Application.DTOs.Responses;

namespace Booking.Application.Interfaces;

public interface IBookingService
{
    Task<BookingResponse> CreateAsync(CreateBookingRequest request);

    Task<BookingResponse?> GetByIdAsync(int id);
    Task<List<BookingResponse>> GetByUserIdAsync(int userId);
    Task<List<BookingResponse>> GetByLotIdAsync(int lotId);
    Task<List<BookingResponse>> GetAllAsync();

    Task<bool> CancelAsync(int id, CancelBookingRequest request);
    Task<bool> CheckInAsync(int id);
    Task<bool> CheckOutAsync(int id, CheckOutRequest request);
    Task<bool> ExtendAsync(int id, ExtendBookingRequest request);

    Task<FarePreviewResponse?> GetFarePreviewAsync(int id);
}