
namespace ParkingLotSystem.CalculationStrategies;

public class BaseFareStrategy : IFareStrategy
{
    private const decimal SmallVehicleRate = 1.0m;
    private const decimal MediumVehicleRate = 1.3m;
    private const decimal LargeVehicleRate = 1.8m;

    public decimal CalculateFare(Ticket ticket, decimal fare)
    {
        var duration = ticket.CalculateParkingDuration();

        var fee = ticket.Vehicle.Size switch
        {

            VehicleSize.Small => SmallVehicleRate * (decimal)duration.TotalHours,
            VehicleSize.Medium => MediumVehicleRate * (decimal)duration.TotalHours,
            VehicleSize.Large => LargeVehicleRate * (decimal)duration.TotalHours,
            _ => throw new NotImplementedException(),
        };

        return fare += fee;
    }
}