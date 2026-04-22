using ParkingLotSystem.CalculationStrategies;

namespace ParkingLotSystem;

public static class Program
{
    public static void Main()
    {
        var vehicle1 = new Car("CAR 282 BA");
        var vehicle2 = new Motorcycle("MOTO 001 NO");

        var parkingSpots = new List<ParkingSpot>
        {
            new OversizedSpot(1),
            new RegularSpot(2),
            new OversizedSpot(3),
            new CompactSpot(4),
            new RegularSpot(5),
            new OversizedSpot(6),
            new CompactSpot(7),
            new CompactSpot(8),
            new RegularSpot(9),
        };
        var pm = new ParkingManager(parkingSpots);

        pm.ParkVehicle(vehicle1);
        pm.ParkVehicle(vehicle2);

        pm.Dump();

        FareCalculator fareCalculator = new FareCalculator(new PeakHoursFareStrategy());
        var ticket1 = new Ticket("TICKET-001", vehicle1, parkingSpots[1], DateTime.Now.AddHours(-3), DateTime.Now);
        var ticket2 = new Ticket("TICKET-002", vehicle2, parkingSpots[3], DateTime.Now.AddHours(-1), DateTime.Now);

        fareCalculator.OnFeeCalculated += (sender, args) =>
        {
            Console.WriteLine($"Fare calculated for {args.Ticket.Vehicle.LicensePlate}: {args.Fare:C}");
        };

        fareCalculator.CalculateFare(ticket1);
        fareCalculator.CalculateFare(ticket2);
    }
}



