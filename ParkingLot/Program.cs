using ParkingLotSystem.CalculationStrategies;
using ParkingLotSystem.ParkingSpots;
using ParkingLotSystem.Vehicles;

namespace ParkingLotSystem;

public class Program
{
    public static void Main()
    {
        var logger = new ConsoleLogger<Program>();
        logger.LogInformation(string.Empty);

        IDateTimeService dateTimeService = new SystemDateTimeService();

        var blockerCar = new Car("CAR 111 VIP");
        var car = new Car("CAR 282 BA");
        var motorcycle = new Motorcycle("MOTO 001 NO");

        var parkingSpots = new List<ParkingSpot>
        {
            new OversizedSpot(1),
            new RegularSpot(2),
            new OversizedSpot(3),
            new CompactSpot(4),
            new RegularSpot(5, isVip: true),
            // new OversizedSpot(6),
            new HandicappedSpot(7)
        };

        parkingSpots[2].MarkUnderConstruction();
        parkingSpots[5].MarkCleaning();

        var parkingManager = new ParkingManager(parkingSpots, new ConsoleLogger<ParkingManager>());

        FareCalculator fareCalculator = new FareCalculator([
            new BaseFareStrategy(),
            new PeakHoursFareStrategy(),
            new VipSpotFareStrategy(),
            new BonusFareStrategy(new HolidayService())
        ]);
        fareCalculator.OnFeeCalculated += (sender, args) =>
        {
            logger.LogInformation($"Fare calculated for {args.Ticket.Vehicle.LicensePlate}: {args.Fare:C}");
        };

        var parkingLot = new ParkingLot(parkingManager, fareCalculator, dateTimeService, new ConsoleLogger<ParkingLot>());

        // Occupy the first regular spot so the next medium vehicle is forced to VIP spot #5.
        var blockerTicket = parkingLot.EnterVehicle(blockerCar, dateTimeService.Now.AddHours(1));
        var carTicket = parkingLot.EnterVehicle(car, dateTimeService.Now.AddHours(5));
        var motorcycleTicket = parkingLot.EnterVehicle(motorcycle, dateTimeService.Now.AddHours(5));

        parkingLot.ParkingManager.Dump();

        parkingSpots[5].MarkAvailable();
        logger.LogInformation(string.Empty);
        logger.LogInformation($"Spot {parkingSpots[5].SpotNumber} was cleaned and is now {parkingSpots[5].Status}.");

        logger.LogInformation(string.Empty);
        logger.LogInformation("Expected: CAR 282 BA should be parked on VIP spot and billed with x2 multiplier.");

        parkingLot.LeaveVehicle(carTicket!);

        logger.LogInformation(string.Empty);

        parkingLot.LeaveVehicle(motorcycleTicket!);
        parkingLot.LeaveVehicle(blockerTicket!);
    }
}








