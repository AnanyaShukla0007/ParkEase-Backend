namespace ParkingLot.Application.DTOs;

public class NearbyParkingLotResponse : ParkingLotResponse
{
    public double DistanceKm { get; set; }

    public int EstimatedMinutes { get; set; }
}