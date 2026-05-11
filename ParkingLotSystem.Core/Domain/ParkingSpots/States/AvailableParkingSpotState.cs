using ParkingLotSystem.Vehicles;
using ParkingLotSystem.Core.Domain;

namespace ParkingLotSystem.ParkingSpots.States;

internal sealed class AvailableParkingSpotState : IParkingSpotState
{
    public static AvailableParkingSpotState Instance { get; } = new();

    private AvailableParkingSpotState()
    {
    }

    public Vehicle? AssignedVehicle => null;
    public bool IsAvailable => true;
    public ParkingSpotStatus Status => ParkingSpotStatus.Available;

    public void AssignVehicle(ParkingSpot spot, Vehicle vehicle)
    {
        if (vehicle.Size > spot.Size)
        {
            throw new DomainError($"Vehicle {vehicle} cannot be parked into spot {spot.SpotNumber}");
        }

        spot.SetState(new OccupiedParkingSpotState(vehicle));
    }

    public void Release(ParkingSpot spot)
    {
    }
}

