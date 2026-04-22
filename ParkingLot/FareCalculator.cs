namespace ParkingLotSystem;

public class FareCalculator
{
    private readonly ICollection<IFareStrategy> _fareStrategies = [];

    public event EventHandler<FareCalculatedEventArgs>? OnFeeCalculated;

    public FareCalculator(ICollection<IFareStrategy> fareStrategies)
    {
        _fareStrategies = fareStrategies;
    }

    public decimal CalculateFare(Ticket ticket, decimal inputFare = 0)
    {
        var fare = inputFare;

        foreach (var strategy in _fareStrategies)
        {
            fare = strategy.CalculateFare(ticket, fare);
        }

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