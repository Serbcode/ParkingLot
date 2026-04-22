using ParkingLotSystem.CalculationStrategies;

namespace ParkingLotSystem;

public class Program
{
    public static void Main()
    {
        Console.WriteLine();
        IDateTimeService dateTimeService = new SystemDateTimeService();

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
        var parkingManager = new ParkingManager(parkingSpots);

        FareCalculator fareCalculator = new FareCalculator([new BaseFareStrategy(), new PeakHoursFareStrategy()]);
        fareCalculator.OnFeeCalculated += (sender, args) =>
        {
            Console.WriteLine($"Fare calculated for {args.Ticket.Vehicle.LicensePlate}: {args.Fare:C}");
        };

        var lot = new ParkingLot(parkingManager, fareCalculator);

        var ticket1 = lot.EnterVehicle(vehicle1, DateTime.Now, DateTime.Now.AddHours(5));
        var ticket2 = lot.EnterVehicle(vehicle2, DateTime.Now, DateTime.Now.AddHours(5));

        lot.ParkingManager.Dump();

        lot.LeaveVehicle(ticket1!);

        Console.WriteLine();

        lot.LeaveVehicle(ticket2!);
    }
}



