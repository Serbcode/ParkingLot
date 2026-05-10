using ParkingLotSystem.Vehicles;

namespace ParkingLotSystem.ParkingSpots.States;

internal sealed class UnderConstructionParkingSpotState : IParkingSpotState
{
    public static UnderConstructionParkingSpotState Instance { get; } = new();

    private UnderConstructionParkingSpotState()
    {
    }

    public Vehicle? AssignedVehicle => null;
    public bool IsAvailable => false;
    public ParkingSpotStatus Status => ParkingSpotStatus.UnderConstruction;

    public void AssignVehicle(ParkingSpot spot, Vehicle vehicle)
    {
        throw new ApplicationException($"The spot {spot.SpotNumber} is under construction!");
    }

    public void Release(ParkingSpot spot)
    {
    }
}
