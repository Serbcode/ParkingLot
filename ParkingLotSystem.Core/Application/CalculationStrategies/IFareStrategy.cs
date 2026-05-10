namespace ParkingLotSystem.CalculationStrategies;

public interface IFareStrategy
{
    decimal CalculateFare(Ticket ticket, decimal fare);
}