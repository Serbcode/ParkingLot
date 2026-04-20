namespace ParkingLotSystem;

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

    public bool IsAvailable => AssignedVehicle == null;

    public virtual void Release()
    {
        AssignedVehicle = null;
    }

    public override string ToString()
    {
        return $"Spot {SpotNumber} ({Size}) - {(IsAvailable ? "Available" : $"Occupied by {AssignedVehicle}")}";
    }
}

public class CompactSpot(int SpotNumber) : ParkingSpot(SpotNumber, VehicleSize.Small);

public class OversizedSpot(int SpotNumber) : ParkingSpot(SpotNumber, VehicleSize.Large);

public class RegularSpot(int SpotNumber) : ParkingSpot(SpotNumber, VehicleSize.Medium);
