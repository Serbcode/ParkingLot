using ParkingLotSystem.Vehicles;

namespace ParkingLotSystem.ParkingSpots;

public class HandicappedSpot(int spotNumber) : ParkingSpot(spotNumber, VehicleSize.Small);
