using ParkingLotSystem.Vehicles;

namespace ParkingLotSystem.ParkingSpots;

public class CompactSpot(int spotNumber, bool isVip = false) : ParkingSpot(spotNumber, VehicleSize.Small, isVip);
