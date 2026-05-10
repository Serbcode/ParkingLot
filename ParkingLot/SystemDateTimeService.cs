namespace ParkingLotSystem;

public sealed class SystemDateTimeService : IDateTimeService
{
    // Fixed demo time keeps the console sample deterministic.
    public DateTime Now => new(2026, 4, 22, 10, 0, 0);
}
