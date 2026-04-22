namespace ParkingLotSystem.CalculationStrategies;

public class PeakHoursFareStrategy : IFareStrategy
{
    public decimal CalculateFare(Ticket ticket)
    {
        var baseFare = new BaseFareStrategy().CalculateFare(ticket);
        var entryHour = ticket.EntryTime.Hour;

        // Define peak hours (e.g., 8 AM to 6 PM)
        if (entryHour >= 8 && entryHour < 18)
        {
            return baseFare * 1.5m; // 50% surcharge during peak hours
        }

        return baseFare;
    }
}