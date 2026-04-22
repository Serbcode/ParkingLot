namespace ParkingLotSystem;

public class FareCalculator
{
    private readonly IFareStrategy _fareStrategy;

    public event EventHandler<decimal>? OnFeeCalculated;

    public FareCalculator(IFareStrategy fareStrategy)
    {
        _fareStrategy = fareStrategy;
    }

    public decimal CalculateFare(Ticket ticket, decimal? inputFare = null)
    {
        var fare = _fareStrategy.CalculateFare(ticket) + (inputFare ?? 0);
        OnFeeCalculated?.Invoke(this, fare);
        return fare;
    }

    public decimal CalculateFare(Ticket ticket, Func<Ticket, decimal> customFareFunc)
    {
        var fare = customFareFunc(ticket);
        OnFeeCalculated?.Invoke(this, fare);
        return fare;
    }
}