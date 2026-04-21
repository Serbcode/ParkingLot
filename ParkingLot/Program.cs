namespace ParkingLotSystem;

public static class Program
{
    public static void Main()
    {
        var vehicle1 = new Car("IM 282 BA");
        var vehicle2 = new Motorcycle("VA 001 NO");

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
        var pm = new ParkingManager(parkingSpots);

        pm.ParkVehicle(vehicle1);
        pm.ParkVehicle(vehicle2);

        pm.Dump();
    }
}



