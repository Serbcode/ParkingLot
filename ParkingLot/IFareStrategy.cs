namespace ParkingLotSystem;

public interface IFareStrategy
{
    decimal CalculateFare(Ticket ticket);
}