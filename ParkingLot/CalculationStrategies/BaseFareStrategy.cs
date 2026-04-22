
namespace ParkingLotSystem.CalculationStrategies;

public class BaseFareStrategy : IFareStrategy
{
    public decimal CalculateFare(Ticket ticket)
    {
        var duration = ticket.CalculateParkingDuration();
        return (decimal)duration.TotalHours * 5; // Base fare: $5 per hour
    }
}