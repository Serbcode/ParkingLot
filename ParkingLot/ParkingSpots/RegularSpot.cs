using ParkingLotSystem.Vehicles;

namespace ParkingLotSystem.ParkingSpots;

public class RegularSpot(int spotNumber) : ParkingSpot(spotNumber, VehicleSize.Medium);
