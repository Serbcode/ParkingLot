namespace ParkingLotSystem;

public static class Program
{
    public static void Main()
    {
        var vehicle1 = new Car("ABC123");
        var vehicle2 = new Motorcycle("XYZ789");
        Console.WriteLine(string.Join(Environment.NewLine, vehicle1, vehicle2));
    }
}

/// <summary>
/// manages a collection of spots and handles assignments.
/// </summary>
class ParkingLot { }

/// <summary>
/// stores entry time and calculates the fee.
/// </summary>
class Ticket { }




