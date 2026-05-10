using ParkingLotSystem.Vehicles;

namespace ParkingLotSystem.ParkingSpots.States;

internal interface IParkingSpotState
{
    Vehicle? AssignedVehicle { get; }
    bool IsAvailable { get; }
    ParkingSpotStatus Status { get; }

    void AssignVehicle(ParkingSpot spot, Vehicle vehicle);
    void Release(ParkingSpot spot);
}
