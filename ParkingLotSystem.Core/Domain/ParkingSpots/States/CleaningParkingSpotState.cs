using ParkingLotSystem.Vehicles;

namespace ParkingLotSystem.ParkingSpots.States;

internal sealed class CleaningParkingSpotState : IParkingSpotState
{
    public static CleaningParkingSpotState Instance { get; } = new();

    private CleaningParkingSpotState()
    {
    }

    public Vehicle? AssignedVehicle => null;
    public bool IsAvailable => false;
    public ParkingSpotStatus Status => ParkingSpotStatus.Cleaning;

    public void AssignVehicle(ParkingSpot spot, Vehicle vehicle)
    {
        throw new ApplicationException($"The spot {spot.SpotNumber} is being cleaned!");
    }

    public void Release(ParkingSpot spot)
    {
    }
}
