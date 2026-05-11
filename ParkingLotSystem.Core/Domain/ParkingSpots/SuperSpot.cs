using ParkingLotSystem.Vehicles;

namespace ParkingLotSystem.ParkingSpots;

public class SuperSpot(int spotNumber, bool isVip = false) : ParkingSpot(spotNumber, VehicleSize.SuperCar, isVip)
{
}
