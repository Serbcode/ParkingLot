using ParkingLotSystem.Vehicles;

namespace ParkingLotSystem.ParkingSpots;

public class OversizedSpot(int spotNumber) : ParkingSpot(spotNumber, VehicleSize.Large);
