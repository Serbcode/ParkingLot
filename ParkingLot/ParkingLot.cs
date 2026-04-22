namespace ParkingLotSystem;

public class ParkingLot
{
    public readonly ParkingManager ParkingManager;
    private readonly FareCalculator _fareCalculator;

    public ParkingLot(ParkingManager parkingManager, FareCalculator fareCalculator)
    {
        ParkingManager = parkingManager;
        _fareCalculator = fareCalculator;
    }

    public Ticket? EnterVehicle(Vehicle vehicle, DateTime enterTime, DateTime exitTime)
    {
        var spot = ParkingManager.ParkVehicle(vehicle);
        if (spot is null)
        {
            Console.WriteLine($"Unable to park vehicle {vehicle}. No suitable spot available!");
            return null;
        }

        return new Ticket(Guid.NewGuid().ToString(), vehicle, spot, enterTime, exitTime);
    }

    public void LeaveVehicle(Ticket ticket)
    {
        var fee = _fareCalculator.CalculateFare(ticket);
        Console.WriteLine($"Vehicle {ticket.Vehicle} is leaving. Total fee: {fee:C}");

        ParkingManager.ReleaseVehicle(ticket.Vehicle.LicensePlate);
    }
}
