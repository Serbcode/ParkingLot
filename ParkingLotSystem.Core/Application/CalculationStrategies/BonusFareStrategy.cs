namespace ParkingLotSystem.CalculationStrategies;

public class BonusFareStrategy : IFareStrategy
{
    private const decimal HolidayDiscountMultiplier = 0.95m;

    private readonly IHolidayService _holidayService;

    public BonusFareStrategy(IHolidayService holidayService)
    {
        _holidayService = holidayService;
    }

    public decimal CalculateFare(Ticket ticket, decimal fare)
    {
        if (_holidayService.IsHoliday(ticket.EntryTime))
        {
            return fare * HolidayDiscountMultiplier;
        }

        return fare;
    }
}
