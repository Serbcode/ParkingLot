using ParkingLotSystem.Vehicles;

namespace ParkingLotSystem.ParkingSpots;

public class HandicappedSpot(int spotNumber, bool isVip = false) : ParkingSpot(spotNumber, VehicleSize.Small, isVip);
