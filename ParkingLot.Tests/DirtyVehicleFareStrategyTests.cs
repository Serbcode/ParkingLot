using System;
using ParkingLotSystem;
using ParkingLotSystem.CalculationStrategies;
using ParkingLotSystem.Core.Domain;
using ParkingLotSystem.Vehicles;
using Xunit;

namespace ParkingLot.Tests;

public class DirtyVehicleFareStrategyTests
{
    [Fact]
    public void CalculateFare_ShouldAdd5PercentIfVehicleIsDirty()
    {
        // Arrange
        var ticket = new Ticket(
            TicketNumber: "T1",
            Vehicle: new Car("D01", IsDirty: true),      // dirty vehicle
            Spot: null!,   // not needed for fare calculation
            EntryTime: DateTime.UtcNow.AddHours(-1),
            ExitTime: DateTime.UtcNow);

        var baseFare = 10m; // base fare before surcharge
        var strategy = new DirtyVehicleFareStrategy();

        // Act
        var finalFare = strategy.CalculateFare(ticket, baseFare);

        // Assert
        var expectedFare = baseFare * 1.05m; // +5%
        Assert.Equal(expectedFare, finalFare);
    }

    [Fact]
    public void CalculateFare_ShouldNotChangeFareIfVehicleIsClean()
    {
        // Arrange
        var ticket = new Ticket(
            TicketNumber: "T2",
            Vehicle: new Car("D02", IsDirty: false), // clean vehicle
            Spot: null!,
            EntryTime: DateTime.UtcNow.AddHours(-1),
            ExitTime: DateTime.UtcNow);

        var baseFare = 12m;
        var strategy = new DirtyVehicleFareStrategy();

        // Act
        var finalFare = strategy.CalculateFare(ticket, baseFare);

        // Assert
        Assert.Equal(baseFare, finalFare);
    }
}
