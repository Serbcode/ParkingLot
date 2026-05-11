using ParkingLotSystem.Vehicles;
using ParkingLotSystem.Core.Domain;

namespace ParkingLotSystem.CalculationStrategies;

public class DirtyVehicleFareStrategy : IFareStrategy
{
    public decimal CalculateFare(Ticket ticket, decimal fare)
    {
        // Check if the vehicle is dirty
        if (ticket.Vehicle.IsDirty)
        {
            // Apply a 5% surcharge
            fare += fare * 0.05m;
        }

        return fare;
    }
}