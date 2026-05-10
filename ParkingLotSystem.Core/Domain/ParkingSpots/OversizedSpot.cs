using ParkingLotSystem.Vehicles;

namespace ParkingLotSystem.ParkingSpots;

public class OversizedSpot(int spotNumber, bool isVip = false) : ParkingSpot(spotNumber, VehicleSize.Large, isVip);
