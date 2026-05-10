using ParkingLotSystem;
using ParkingLotSystem.CalculationStrategies;
using ParkingLotSystem.ParkingSpots;
using ParkingLotSystem.Vehicles;

namespace ParkingLot.Tests;

public class FareCalculatorTests
{
    [Fact]
    public void CalculateFare_AppliesBasePeakVipAndHolidayStrategiesInOrder()
    {
        var ticket = new Ticket(
            TicketNumber: "T-1",
            Vehicle: new Car("CAR-1"),
            Spot: new RegularSpot(1, isVip: true),
            EntryTime: new DateTime(2026, 4, 25, 10, 0, 0),
            ExitTime: new DateTime(2026, 4, 25, 12, 0, 0));

        var calculator = new FareCalculator([
            new BaseFareStrategy(),
            new PeakHoursFareStrategy(),
            new VipSpotFareStrategy(),
            new BonusFareStrategy(new HolidayService())
        ]);

        var fare = calculator.CalculateFare(ticket);

        Assert.Equal(7.41m, fare);
    }

    [Fact]
    public void CalculateFare_RaisesFeeCalculatedEvent()
    {
        var ticket = new Ticket(
            TicketNumber: "T-2",
            Vehicle: new Motorcycle("MOTO-1"),
            Spot: new CompactSpot(1),
            EntryTime: new DateTime(2026, 4, 22, 6, 0, 0),
            ExitTime: new DateTime(2026, 4, 22, 8, 0, 0));

        var calculator = new FareCalculator([new BaseFareStrategy()]);
        FareCalculatedEventArgs? eventArgs = null;
        calculator.OnFeeCalculated += (_, args) => eventArgs = args;

        var fare = calculator.CalculateFare(ticket);

        Assert.Equal(2.0m, fare);
        Assert.NotNull(eventArgs);
        Assert.Equal(fare, eventArgs.Fare);
        Assert.Same(ticket, eventArgs.Ticket);
    }

    [Fact]
    public void HolidayService_UsesConfiguredHolidayDates()
    {
        var holidayService = new HolidayService(
            [new DateOnly(2026, 1, 1)],
            includeWeekends: false);

        Assert.True(holidayService.IsHoliday(new DateTime(2026, 1, 1, 9, 0, 0)));
        Assert.False(holidayService.IsHoliday(new DateTime(2026, 1, 2, 9, 0, 0)));
    }
}
