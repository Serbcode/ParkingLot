using ParkingLotSystem;
using ParkingLotSystem.ParkingSpots;
using ParkingLotSystem.Vehicles;

namespace ParkingLot.Tests;

public class ParkingManagerTests
{
    [Fact]
    public void ParkVehicle_UsesExactSizeSpotBeforeLargerSpot()
    {
        var car = new Car("CAR-1");
        var oversizedSpot = new OversizedSpot(1);
        var regularSpot = new RegularSpot(2);
        var manager = new ParkingManager([oversizedSpot, regularSpot]);

        var spot = manager.ParkVehicle(car);

        Assert.Same(regularSpot, spot);
        Assert.Same(car, regularSpot.AssignedVehicle);
        Assert.Null(oversizedSpot.AssignedVehicle);
    }

    [Fact]
    public void ParkVehicle_UsesLargerSpotWhenExactSizeSpotIsUnavailable()
    {
        var firstCar = new Car("CAR-1");
        var secondCar = new Car("CAR-2");
        var regularSpot = new RegularSpot(1);
        var oversizedSpot = new OversizedSpot(2);
        var manager = new ParkingManager([regularSpot, oversizedSpot]);

        manager.ParkVehicle(firstCar);
        var spot = manager.ParkVehicle(secondCar);

        Assert.Same(oversizedSpot, spot);
        Assert.Same(secondCar, oversizedSpot.AssignedVehicle);
    }

    [Fact]
    public void ParkVehicle_ReturnsNullWhenNoSuitableSpotExists()
    {
        var manager = new ParkingManager([new CompactSpot(1)]);

        var spot = manager.ParkVehicle(new Truck("TRUCK-1"));

        Assert.Null(spot);
    }

    [Fact]
    public void ReleaseVehicle_ReleasesMatchingSpot()
    {
        var car = new Car("CAR-1");
        var spot = new RegularSpot(1);
        var manager = new ParkingManager([spot]);
        manager.ParkVehicle(car);

        var released = manager.ReleaseVehicle(car.LicensePlate);

        Assert.True(released);
        Assert.Null(spot.AssignedVehicle);
        Assert.Empty(manager.ParkedVehicles);
    }
}
