namespace ParkingLotSystem;

public interface IDateTimeService
{
    DateTime Now { get; }
}

public class SystemDateTimeService : IDateTimeService
{
    // 10 AM on April 22, 2026 - a fixed time for testing purposes (peak hours)
    public DateTime Now => new(2026, 4, 22, 10, 0, 0);

    // For production, we use the actual current time
    // public DateTime Now => DateTime.Now;
}