using ParkingLotSystem.Vehicles;

namespace ParkingLotSystem.ParkingSpots;

public class CompactSpot(int spotNumber) : ParkingSpot(spotNumber, VehicleSize.Small);
