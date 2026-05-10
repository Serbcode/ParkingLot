namespace ParkingLotSystem.CalculationStrategies;

public class VipSpotFareStrategy : IFareStrategy
{
    private const decimal VipMultiplier = 2m;

    public decimal CalculateFare(Ticket ticket, decimal fare)
    {
        return ticket.Spot.IsVip ? fare * VipMultiplier : fare;
    }
}