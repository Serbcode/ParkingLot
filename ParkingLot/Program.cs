namespace ParkingLotSystem;

public static class Program
{
    public static void Main()
    {
        var vehicle1 = new Car("ABC123");
        var vehicle2 = new Motorcycle("XYZ789");

        var parkingSpots = new List<ParkingSpot>
        {
            new CompactSpot(1),
            new RegularSpot(2),
            new OversizedSpot(3)
        };
        var pm = new ParkingManager(parkingSpots);

        pm.ParkVehicle(vehicle1);
        pm.ParkVehicle(vehicle2);
    }
}



