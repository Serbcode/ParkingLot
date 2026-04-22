namespace ParkingLotSystem;

public class FareCalculator
{
    private readonly IFareStrategy _fareStrategy;

    public event EventHandler<FareCalculatedEventArgs>? OnFeeCalculated;

    public FareCalculator(IFareStrategy fareStrategy)
    {
        _fareStrategy = fareStrategy;
    }

    public decimal CalculateFare(Ticket ticket, decimal? inputFare = null)
    {
        var fare = _fareStrategy.CalculateFare(ticket) + (inputFare ?? 0);
        OnFeeCalculated?.Invoke(this, new FareCalculatedEventArgs { Fare = fare, Ticket = ticket });
        return fare;
    }

    public decimal CalculateFare(Ticket ticket, Func<Ticket, decimal> customFareFunc)
    {
        var fare = customFareFunc(ticket);
        OnFeeCalculated?.Invoke(this, new FareCalculatedEventArgs { Fare = fare, Ticket = ticket });
        return fare;
    }
}

public class FareCalculatedEventArgs : EventArgs
{
    public decimal Fare { get; set; }
    public required Ticket Ticket { get; set; }
}