using ParkingLotSystem;
using ParkingLotSystem.CalculationStrategies;
using ParkingLotSystem.ParkingSpots;
using ParkingLotSystem.Vehicles;

namespace ParkingLot.Tests;

public class ParkingLotTests
{
    [Fact]
    public void EnterVehicle_ReturnsTicketWithAssignedSpotAndCurrentEntryTime()
    {
        var now = new DateTime(2026, 4, 22, 10, 0, 0);
        var dateTimeService = new StubDateTimeService(now);
        var manager = new ParkingManager([new RegularSpot(1)]);
        var fareCalculator = new FareCalculator([new BaseFareStrategy()]);
        var parkingLot = new ParkingLotSystem.ParkingLot(manager, fareCalculator, dateTimeService);
        var car = new Car("CAR-1");

        var ticket = parkingLot.EnterVehicle(car, now.AddHours(2));

        Assert.NotNull(ticket);
        Assert.Equal(car, ticket.Vehicle);
        Assert.Equal(now, ticket.EntryTime);
        Assert.Equal(now.AddHours(2), ticket.ExitTime);
        Assert.Same(car, ticket.Spot.AssignedVehicle);
    }

    [Fact]
    public void LeaveVehicle_UsesProvidedExitTimeAndReleasesVehicle()
    {
        var now = new DateTime(2026, 4, 22, 6, 0, 0);
        var dateTimeService = new StubDateTimeService(now);
        var manager = new ParkingManager([new RegularSpot(1)]);
        var fareCalculator = new FareCalculator([new BaseFareStrategy()]);
        var parkingLot = new ParkingLotSystem.ParkingLot(manager, fareCalculator, dateTimeService);
        var ticket = parkingLot.EnterVehicle(new Car("CAR-1"), now.AddHours(1));
        decimal? calculatedFare = null;
        fareCalculator.OnFeeCalculated += (_, args) => calculatedFare = args.Fare;

        parkingLot.LeaveVehicle(ticket!, now.AddHours(3));

        Assert.Equal(3.9m, calculatedFare);
        Assert.Empty(manager.ParkedVehicles);
    }

    private sealed class StubDateTimeService(DateTime now) : IDateTimeService
    {
        public DateTime Now { get; } = now;
    }
}
