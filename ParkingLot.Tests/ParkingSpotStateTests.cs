using ParkingLotSystem.ParkingSpots;
using ParkingLotSystem.Vehicles;

namespace ParkingLot.Tests;

public class ParkingSpotStateTests
{
    [Fact]
    public void NewSpot_StartsAvailable()
    {
        var spot = new RegularSpot(1);

        Assert.Equal(ParkingSpotStatus.Available, spot.Status);
        Assert.True(spot.IsAvailable);
        Assert.False(spot.IsTaken);
        Assert.Null(spot.AssignedVehicle);
    }

    [Fact]
    public void AssignVehicle_MovesSpotToOccupied()
    {
        var car = new Car("CAR-1");
        var spot = new RegularSpot(1);

        spot.AssignVehicle(car);

        Assert.Equal(ParkingSpotStatus.Occupied, spot.Status);
        Assert.False(spot.IsAvailable);
        Assert.True(spot.IsTaken);
        Assert.Same(car, spot.AssignedVehicle);
    }

    [Fact]
    public void Release_MovesOccupiedSpotToAvailable()
    {
        var spot = new RegularSpot(1);
        spot.AssignVehicle(new Car("CAR-1"));

        spot.Release();

        Assert.Equal(ParkingSpotStatus.Available, spot.Status);
        Assert.True(spot.IsAvailable);
        Assert.Null(spot.AssignedVehicle);
    }

    [Fact]
    public void MarkUnderConstruction_MakesSpotUnavailableWithoutAssignedVehicle()
    {
        var spot = new RegularSpot(1);

        spot.MarkUnderConstruction();

        Assert.Equal(ParkingSpotStatus.UnderConstruction, spot.Status);
        Assert.False(spot.IsAvailable);
        Assert.True(spot.IsTaken);
        Assert.Null(spot.AssignedVehicle);
    }

    [Fact]
    public void MarkCleaning_MakesSpotUnavailableWithoutAssignedVehicle()
    {
        var spot = new RegularSpot(1);

        spot.MarkCleaning();

        Assert.Equal(ParkingSpotStatus.Cleaning, spot.Status);
        Assert.False(spot.IsAvailable);
        Assert.True(spot.IsTaken);
        Assert.Null(spot.AssignedVehicle);
    }

    [Fact]
    public void NonAvailableStates_RejectAssignedVehicles()
    {
        var underConstructionSpot = new RegularSpot(1);
        var cleaningSpot = new RegularSpot(2);
        underConstructionSpot.MarkUnderConstruction();
        cleaningSpot.MarkCleaning();

        Assert.Throws<ApplicationException>(() => underConstructionSpot.AssignVehicle(new Car("CAR-1")));
        Assert.Throws<ApplicationException>(() => cleaningSpot.AssignVehicle(new Car("CAR-2")));
    }

    [Fact]
    public void MarkAvailable_ReopensCleaningAndUnderConstructionSpots()
    {
        var underConstructionSpot = new RegularSpot(1);
        var cleaningSpot = new RegularSpot(2);
        underConstructionSpot.MarkUnderConstruction();
        cleaningSpot.MarkCleaning();

        underConstructionSpot.MarkAvailable();
        cleaningSpot.MarkAvailable();

        Assert.Equal(ParkingSpotStatus.Available, underConstructionSpot.Status);
        Assert.Equal(ParkingSpotStatus.Available, cleaningSpot.Status);
        Assert.True(underConstructionSpot.IsAvailable);
        Assert.True(cleaningSpot.IsAvailable);
    }

    [Fact]
    public void MarkingUnavailableState_RequiresReleasedSpot()
    {
        var spot = new RegularSpot(1);
        spot.AssignVehicle(new Car("CAR-1"));

        Assert.Throws<ApplicationException>(spot.MarkCleaning);
        Assert.Throws<ApplicationException>(spot.MarkUnderConstruction);
        Assert.Throws<ApplicationException>(spot.MarkAvailable);
    }
}
