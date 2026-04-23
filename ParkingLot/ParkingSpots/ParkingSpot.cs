using ParkingLotSystem.Vehicles;

namespace ParkingLotSystem.ParkingSpots;

/// <summary>
/// tracks size, availability, and assigned vehicle.
/// </summary>
public class ParkingSpot(int spotNumber, VehicleSize size)
{
    public readonly int SpotNumber = spotNumber;
    public readonly VehicleSize Size = size;

    public Vehicle? AssignedVehicle { get; private set; } = null;

    public virtual void AssignVehicle(Vehicle vehicle)
    {
        if (AssignedVehicle is not null)
        {
            throw new ApplicationException($"The spot {SpotNumber} is already taken!");
        }

        if (vehicle.Size > Size)
        {
            throw new ApplicationException($"Vehicle {vehicle} cannot be parked into spot {SpotNumber}");
        }

        AssignedVehicle = vehicle;
    }

    public bool IsAvailable => AssignedVehicle is null;

    public bool IsTaken => !IsAvailable;

    public virtual void Release()
    {
        AssignedVehicle = null;
    }

    public override string ToString()
    {
        return this switch
        {
            CompactSpot => $"[🏍️- {AssignedVehicle?.LicensePlate}]",
            RegularSpot => $"[🚗 - {AssignedVehicle?.LicensePlate}]",
            OversizedSpot => $"[🚚 - {AssignedVehicle?.LicensePlate}]",
            HandicappedSpot => $"[♿ - {AssignedVehicle?.LicensePlate}]",
            _ => $"[Unknown Spot {SpotNumber}]"
        };
    }
}
