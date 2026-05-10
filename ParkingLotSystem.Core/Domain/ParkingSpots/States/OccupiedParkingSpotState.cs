using ParkingLotSystem.Vehicles;

namespace ParkingLotSystem.ParkingSpots.States;

internal sealed class OccupiedParkingSpotState(Vehicle assignedVehicle) : IParkingSpotState
{
    public Vehicle? AssignedVehicle { get; } = assignedVehicle;
    public bool IsAvailable => false;
    public ParkingSpotStatus Status => ParkingSpotStatus.Occupied;

    public void AssignVehicle(ParkingSpot spot, Vehicle vehicle)
    {
        throw new ApplicationException($"The spot {spot.SpotNumber} is already taken!");
    }

    public void Release(ParkingSpot spot)
    {
        spot.SetState(AvailableParkingSpotState.Instance);
    }
}
