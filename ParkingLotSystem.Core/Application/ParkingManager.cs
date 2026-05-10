using ParkingLotSystem.ParkingSpots;
using ParkingLotSystem.Vehicles;

namespace ParkingLotSystem;

public class ParkingManager
{
    private readonly Lock _parkingLock = new();
    private readonly List<ParkingSpot> _parkingSpots;
    private readonly ILogger<ParkingManager> _logger;

    public ParkingManager(List<ParkingSpot> parkingSpots, ILogger<ParkingManager>? logger = null)
    {
        _parkingSpots = parkingSpots;
        _logger = logger ?? NullLogger<ParkingManager>.Instance;
    }

    public IReadOnlyCollection<Vehicle> ParkedVehicles
    {
        get
        {
            lock (_parkingLock)
            {
                return _parkingSpots
                    .Where(s => s.AssignedVehicle is not null)
                    .Select(s => s.AssignedVehicle!)
                    .ToList();
            }
        }
    }

    public IReadOnlyCollection<ParkingSpot> AvailableSpots
    {
        get
        {
            lock (_parkingLock)
            {
                return _parkingSpots.Where(s => s.IsAvailable).ToList();
            }
        }
    }

    public ParkingSpot? ParkVehicle(Vehicle vehicle)
    {
        lock (_parkingLock)
        {
            var spotMatch = _parkingSpots.FirstOrDefault(s => s.IsAvailable && s.Size == vehicle.Size);
            if (spotMatch is not null)
            {
                spotMatch.AssignVehicle(vehicle);
                _logger.LogInformation($"Parked vehicle {vehicle} in spot {spotMatch.SpotNumber}");
                return spotMatch;
            }

            var validSpot = _parkingSpots.FirstOrDefault(s => s.IsAvailable && s.Size > vehicle.Size);
            if (validSpot is not null)
            {
                validSpot.AssignVehicle(vehicle);
                _logger.LogInformation($"Parked vehicle {vehicle} in spot {validSpot.SpotNumber}");
                return validSpot;
            }
        }

        _logger.LogInformation($"No available spot for vehicle {vehicle}");
        return null;
    }

    public bool ReleaseVehicle(string licensePlate)
    {
        ParkingSpot? spot;

        lock (_parkingLock)
        {
            spot = _parkingSpots.FirstOrDefault(s => s.AssignedVehicle?.LicensePlate == licensePlate);
            if (spot is not null)
            {
                spot.Release();
            }
        }

        if (spot is null)
        {
            _logger.LogInformation($"No vehicle with license plate {licensePlate} found in the parking lot.");
            return false;
        }

        _logger.LogInformation($"Released vehicle with license plate {licensePlate} from spot {spot.SpotNumber}");
        return true;
    }

    public void Dump()
    {
        lock (_parkingLock)
        {
            _logger.LogInformation(string.Join(Environment.NewLine, _parkingSpots.Select(s => s.ToString())));
        }
    }
}
