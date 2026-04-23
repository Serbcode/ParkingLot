using ParkingLotSystem.CalculationStrategies;
using ParkingLotSystem.ParkingSpots;
using ParkingLotSystem.Vehicles;

namespace ParkingLotSystem;

public class Program
{
    public static void Main()
    {
        Console.WriteLine();
        IDateTimeService dateTimeService = new SystemDateTimeService();

        var car = new Car("CAR 282 BA");
        var motorcycle = new Motorcycle("MOTO 001 NO");

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

        var parkingLot = new ParkingLot(parkingManager, fareCalculator, dateTimeService);

        var carTicket = parkingLot.EnterVehicle(car, dateTimeService.Now.AddHours(5));
        var motorcycleTicket = parkingLot.EnterVehicle(motorcycle, dateTimeService.Now.AddHours(5));

        parkingLot.ParkingManager.Dump();

        parkingLot.LeaveVehicle(carTicket!);

        Console.WriteLine();

        parkingLot.LeaveVehicle(motorcycleTicket!);
    }
}



