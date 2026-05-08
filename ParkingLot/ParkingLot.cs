using ParkingLotSystem.Vehicles;

namespace ParkingLotSystem;

public class ParkingLot
{
    public readonly ParkingManager ParkingManager;
    private readonly FareCalculator _fareCalculator;
    private readonly IDateTimeService _dateTimeService;

    public ParkingLot(ParkingManager parkingManager, FareCalculator fareCalculator, IDateTimeService dateTimeService)
    {
        ParkingManager = parkingManager;
        _fareCalculator = fareCalculator;
        _dateTimeService = dateTimeService;
    }

    public Ticket? EnterVehicle(Vehicle vehicle, DateTime exitTime)
    {
        var spot = ParkingManager.ParkVehicle(vehicle);
        if (spot is null)
        {
            Console.WriteLine($"Unable to park vehicle {vehicle}. No suitable spot available!");
            return null;
        }

        if (spot.IsVip)
        {
            Console.WriteLine($"Spot {spot.SpotNumber} is VIP. Fare multiplier x2 will be applied.");
        }

        return new Ticket(Guid.NewGuid().ToString(), vehicle, spot, _dateTimeService.Now, exitTime);
    }

    public void LeaveVehicle(Ticket ticket, DateTime? exitTime = null)
    {
        if (exitTime.HasValue)
        {
            ticket = ticket with { ExitTime = exitTime.Value };
        }

        var fee = _fareCalculator.CalculateFare(ticket);
        Console.WriteLine($"Vehicle {ticket.Vehicle} is leaving. Total fee: {fee:C}");

        ParkingManager.ReleaseVehicle(ticket.Vehicle.LicensePlate);
    }
}
