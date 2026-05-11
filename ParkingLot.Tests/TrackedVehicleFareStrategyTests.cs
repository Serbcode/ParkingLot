using System;
using ParkingLotSystem;
using ParkingLotSystem.CalculationStrategies;
using ParkingLotSystem.Core.Domain;
using ParkingLotSystem.Vehicles;
using Xunit;

namespace ParkingLot.Tests;

public class TrackedVehicleFareStrategyTests
{
    [Fact]
    public void CalculateFare_ShouldDoubleFareIfVehicleIsTracked()
    {
        var ticket = new Ticket(
            TicketNumber: "T1",
            Vehicle: new Car("TRK01", IsTracked: true),
            Spot: null!,
            EntryTime: DateTime.UtcNow.AddHours(-1),
            ExitTime: DateTime.UtcNow);

        var baseFare = 10m;
        var strategy = new TrackedVehicleFareStrategy();

        var finalFare = strategy.CalculateFare(ticket, baseFare);

        Assert.Equal(baseFare * 2m, finalFare);
    }

    [Fact]
    public void CalculateFare_ShouldNotChangeFareIfVehicleIsNotTracked()
    {
        var ticket = new Ticket(
            TicketNumber: "T2",
            Vehicle: new Car("TRK02", IsTracked: false),
            Spot: null!,
            EntryTime: DateTime.UtcNow.AddHours(-1),
            ExitTime: DateTime.UtcNow);

        var baseFare = 12m;
        var strategy = new TrackedVehicleFareStrategy();

        var finalFare = strategy.CalculateFare(ticket, baseFare);

        Assert.Equal(baseFare, finalFare);
    }
}
