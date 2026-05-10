namespace ParkingLotSystem;

public interface IHolidayService
{
    bool IsHoliday(DateTime date);
}

public class HolidayService : IHolidayService
{
    private readonly HashSet<DateOnly> _holidays;
    private readonly bool _includeWeekends;

    public HolidayService(IEnumerable<DateOnly>? holidays = null, bool includeWeekends = true)
    {
        _holidays = holidays?.ToHashSet() ?? [];
        _includeWeekends = includeWeekends;
    }

    public bool IsHoliday(DateTime date)
    {
        return _holidays.Contains(DateOnly.FromDateTime(date)) ||
               (_includeWeekends && IsWeekend(date));
    }

    private static bool IsWeekend(DateTime date) =>
        date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
}
