using ParkingLotSystem.Vehicles;

namespace ParkingLotSystem.ParkingSpots;

public class RegularSpot(int spotNumber, bool isVip = false) : ParkingSpot(spotNumber, VehicleSize.Medium, isVip);
