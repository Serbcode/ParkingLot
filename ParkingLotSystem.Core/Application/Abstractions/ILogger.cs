namespace ParkingLotSystem;

public interface ILogger<T>
{
    void LogInformation(string message);
}

public sealed class NullLogger<T> : ILogger<T>
{
    public static NullLogger<T> Instance { get; } = new();

    private NullLogger()
    {
    }

    public void LogInformation(string message)
    {
    }
}
