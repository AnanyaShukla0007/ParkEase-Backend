using Booking.Application.DTOs.Requests;
using Booking.Application.DTOs.Responses;
using Booking.Application.Interfaces;
using Booking.Application.Mappings;
using Booking.Application.Validators;
using Booking.Domain.Enums;
using BookingEntity = Booking.Domain.Entities.Booking;

namespace Booking.Application.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ISpotClient _spotClient;
    private readonly IParkingLotClient _parkingLotClient;

    public BookingService(
        IBookingRepository bookingRepository,
        ISpotClient spotClient,
        IParkingLotClient parkingLotClient)
    {
        _bookingRepository = bookingRepository;
        _spotClient = spotClient;
        _parkingLotClient = parkingLotClient;
    }

    public async Task<BookingResponse> CreateAsync(CreateBookingRequest request)
    {
        var errors = CreateBookingValidator.Validate(request);

        if (errors.Count > 0)
            throw new Exception(string.Join(" | ", errors));

        var hasOverlap = await _bookingRepository.HasOverlappingBookingAsync(
            request.SpotId,
            request.StartTimeUtc,
            request.EndTimeUtc);

        if (hasOverlap)
            throw new Exception("Spot already booked for selected time.");

        var reserved = await _spotClient.ReserveSpotAsync(request.SpotId);

        if (!reserved)
            throw new Exception("Unable to reserve selected spot.");

        var spot = await _spotClient.GetSpotByIdAsync(request.SpotId);
        var estimatedAmount = request.EstimatedAmount > 0
            ? request.EstimatedAmount
            : CalculateEstimatedAmount(
                request.StartTimeUtc,
                request.EndTimeUtc,
                spot?.PricePerHour ?? 0);

        var lotUpdated = await _parkingLotClient.DecrementAvailableAsync(request.LotId);

        if (!lotUpdated)
        {
            await _spotClient.ReleaseSpotAsync(request.SpotId);
            throw new Exception("Unable to update parking lot availability.");
        }

        var booking = new BookingEntity
        {
            UserId = request.UserId,
            LotId = request.LotId,
            SpotId = request.SpotId,
            VehicleId = request.VehicleId,
            VehiclePlate = request.VehiclePlate,
            BookingType = request.BookingType,
            Status = BookingStatus.Confirmed,
            PaymentState = PaymentState.NotRequired,
            StartTimeUtc = request.StartTimeUtc,
            EndTimeUtc = request.EndTimeUtc,
            EstimatedAmount = estimatedAmount,
            FinalAmount = 0,
            Notes = request.Notes,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _bookingRepository.AddAsync(booking);
        await _bookingRepository.SaveChangesAsync();

        return BookingMappingProfile.ToResponse(booking);
    }

    public async Task<BookingResponse?> GetByIdAsync(int id)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);

        if (booking is null)
            return null;

        return BookingMappingProfile.ToResponse(booking);
    }

    public async Task<List<BookingResponse>> GetByUserIdAsync(int userId)
    {
        var items = await _bookingRepository.GetByUserIdAsync(userId);

        return items
            .Select(BookingMappingProfile.ToResponse)
            .ToList();
    }

    public async Task<List<BookingResponse>> GetByLotIdAsync(int lotId)
    {
        var items = await _bookingRepository.GetByLotIdAsync(lotId);

        return items
            .Select(BookingMappingProfile.ToResponse)
            .ToList();
    }

    public async Task<List<BookingResponse>> GetAllAsync()
    {
        var items = await _bookingRepository.GetAllAsync();

        return items
            .Select(BookingMappingProfile.ToResponse)
            .ToList();
    }

    public async Task<bool> CancelAsync(int id, CancelBookingRequest request)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);

        if (booking is null)
            return false;

        if (booking.Status == BookingStatus.Cancelled ||
            booking.Status == BookingStatus.Completed)
            return false;

        booking.Status = BookingStatus.Cancelled;
        booking.UpdatedAtUtc = DateTime.UtcNow;
        booking.Notes = request.Reason;

        await _spotClient.ReleaseSpotAsync(booking.SpotId);
        await _parkingLotClient.IncrementAvailableAsync(booking.LotId);

        _bookingRepository.Update(booking);
        await _bookingRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CheckInAsync(int id)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);

        if (booking is null)
            return false;

        if (booking.Status != BookingStatus.Confirmed)
            return false;

        var occupied = await _spotClient.OccupySpotAsync(booking.SpotId);

        if (!occupied)
            return false;

        booking.Status = BookingStatus.Active;
        booking.CheckInTimeUtc = DateTime.UtcNow;
        booking.UpdatedAtUtc = DateTime.UtcNow;

        _bookingRepository.Update(booking);
        await _bookingRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CheckOutAsync(int id, CheckOutRequest request)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);

        if (booking is null)
            return false;

        if (booking.Status != BookingStatus.Active)
            return false;

        booking.Status = BookingStatus.Completed;
        booking.CheckOutTimeUtc = DateTime.UtcNow;

        if (booking.EstimatedAmount <= 0)
        {
            var spot = await _spotClient.GetSpotByIdAsync(booking.SpotId);
            booking.EstimatedAmount = CalculateEstimatedAmount(
                booking.StartTimeUtc,
                booking.EndTimeUtc,
                spot?.PricePerHour ?? 0);
        }

        booking.FinalAmount = request.FinalAmount > 0
            ? request.FinalAmount
            : CalculateFinalAmount(booking, booking.CheckOutTimeUtc.Value);
        booking.UpdatedAtUtc = DateTime.UtcNow;

        await _spotClient.ReleaseSpotAsync(booking.SpotId);
        await _parkingLotClient.IncrementAvailableAsync(booking.LotId);

        _bookingRepository.Update(booking);
        await _bookingRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ExtendAsync(int id, ExtendBookingRequest request)
    {
        var errors = ExtendBookingValidator.Validate(request);

        if (errors.Count > 0)
            throw new Exception(string.Join(" | ", errors));

        var booking = await _bookingRepository.GetByIdAsync(id);

        if (booking is null)
            return false;

        if (booking.Status != BookingStatus.Confirmed &&
            booking.Status != BookingStatus.Active)
            return false;

        if (request.NewEndTimeUtc <= booking.EndTimeUtc)
            return false;

        booking.EndTimeUtc = request.NewEndTimeUtc;
        booking.UpdatedAtUtc = DateTime.UtcNow;

        _bookingRepository.Update(booking);
        await _bookingRepository.SaveChangesAsync();

        return true;
    }

    public async Task<FarePreviewResponse?> GetFarePreviewAsync(int id)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);

        if (booking is null)
            return null;

        var start = booking.CheckInTimeUtc ?? booking.StartTimeUtc;
        var end = booking.CheckOutTimeUtc ?? DateTime.UtcNow;

        if (end < start)
            end = start;

        if (booking.EstimatedAmount <= 0)
        {
            var spot = await _spotClient.GetSpotByIdAsync(booking.SpotId);
            booking.EstimatedAmount = CalculateEstimatedAmount(
                booking.StartTimeUtc,
                booking.EndTimeUtc,
                spot?.PricePerHour ?? 0);
        }

        if (booking.Status == BookingStatus.Completed && booking.FinalAmount > 0)
        {
            return new FarePreviewResponse
            {
                BookingId = booking.Id,
                EstimatedAmount = booking.EstimatedAmount,
                FinalAmount = decimal.Round(booking.FinalAmount, 2)
            };
        }

        return new FarePreviewResponse
        {
            BookingId = booking.Id,
            EstimatedAmount = booking.EstimatedAmount,
            FinalAmount = decimal.Round(CalculateFinalAmount(booking, end), 2)
        };
    }

    private static decimal CalculateEstimatedAmount(
        DateTime startUtc,
        DateTime endUtc,
        decimal pricePerHour)
    {
        if (pricePerHour <= 0)
            return 0;

        var bookedHours = Math.Ceiling((endUtc - startUtc).TotalHours);
        if (bookedHours < 1)
            bookedHours = 1;

        return decimal.Round(pricePerHour * (decimal)bookedHours, 2);
    }

    private static decimal CalculateFinalAmount(BookingEntity booking, DateTime endUtc)
    {
        var startUtc = booking.CheckInTimeUtc ?? booking.StartTimeUtc;
        if (endUtc < startUtc)
            endUtc = startUtc;

        var bookedHours = Math.Ceiling((booking.EndTimeUtc - booking.StartTimeUtc).TotalHours);
        if (bookedHours < 1)
            bookedHours = 1;

        var hourlyRate = booking.EstimatedAmount > 0
            ? booking.EstimatedAmount / (decimal)bookedHours
            : 0;

        var parkedMinutes = (endUtc - startUtc).TotalMinutes;
        if (parkedMinutes <= 30)
            return decimal.Round(hourlyRate / 2, 2);

        var billableHours = Math.Ceiling(parkedMinutes / 60);
        if (billableHours < 1)
            billableHours = 1;

        return decimal.Round(hourlyRate * (decimal)billableHours, 2);
    }

    public async Task<LastParkedResponse?> GetLastParkedAsync(int userId)
    {
        var bookings = await _bookingRepository.GetByUserIdAsync(userId);

        var booking = bookings
            .Where(x => x.CheckInTimeUtc.HasValue)
            .OrderByDescending(x => x.CheckInTimeUtc)
            .ThenByDescending(x => x.CreatedAtUtc)
            .FirstOrDefault();

        if (booking is null)
            return null;

        var lot = await _parkingLotClient.GetParkingLotByIdAsync(booking.LotId);
        var spot = await _spotClient.GetSpotByIdAsync(booking.SpotId);

        return new LastParkedResponse
        {
            BookingId = booking.Id,
            LotId = booking.LotId,
            SpotId = booking.SpotId,
            LotName = lot?.Name ?? $"Lot {booking.LotId}",
            Floor = spot?.Floor.ToString() ?? string.Empty,
            Section = null,
            SpotNumber = spot?.SpotNumber ?? booking.SpotId.ToString(),
            ParkedAtUtc = booking.CheckInTimeUtc!.Value,
            Note = string.IsNullOrWhiteSpace(booking.Notes) ? null : booking.Notes,
            PhotoUrl = null
        };
    }
}
