namespace ParkingLotSystem.CalculationStrategies;

public class PeakHoursFareStrategy : IFareStrategy
{
    private const decimal PeakHourSurcharge = 1.5m;

    public decimal CalculateFare(Ticket ticket, decimal fare)
    {
        return IsPeakHour(ticket.EntryTime) ? fare * PeakHourSurcharge : fare;
    }

    private static bool IsPeakHour(DateTime time) =>
        time.Hour >= 8 && time.Hour < 18;
}