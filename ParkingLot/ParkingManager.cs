namespace ParkingLotSystem;

public class ParkingManager
{
    private readonly List<ParkingSpot> _parkingSpots;

    public ParkingManager(List<ParkingSpot> parkingSpots)
    {
        _parkingSpots = parkingSpots;
    }

    public IReadOnlyCollection<Vehicle> ParkedVehicles =>
        _parkingSpots.Where(s => s.IsTaken).Select(s => s.AssignedVehicle!).ToList();

    public IReadOnlyCollection<ParkingSpot> AvailableSpots =>
        _parkingSpots.Where(s => s.IsAvailable).ToList();

    public bool ParkVehicle(Vehicle vehicle)
    {
        var spot = _parkingSpots.FirstOrDefault(s => s.IsAvailable && s.Size >= vehicle.Size);
        if (spot is null)
        {
            Console.WriteLine($"No available spot for vehicle {vehicle}");
            return false;
        }

        spot.AssignVehicle(vehicle);
        Console.WriteLine($"Parked vehicle {vehicle} in spot {spot.SpotNumber}");
        return true;
    }

    public bool ReleaseVehicle(string licensePlate)
    {
        var spot = _parkingSpots.FirstOrDefault(s => s.AssignedVehicle?.LicensePlate == licensePlate);
        if (spot is null)
        {
            Console.WriteLine($"No vehicle with license plate {licensePlate} found in the parking lot.");
            return false;
        }

        spot.Release();
        Console.WriteLine($"Released vehicle with license plate {licensePlate} from spot {spot.SpotNumber}");
        return true;
    }
}