namespace ParkingLotSystem;

public sealed class ConsoleLogger<T> : ILogger<T>
{
    public void LogInformation(string message)
    {
        Console.WriteLine(message);
    }
}
