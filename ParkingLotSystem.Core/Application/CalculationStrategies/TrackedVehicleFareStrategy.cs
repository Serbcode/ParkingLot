using ParkingLotSystem.Vehicles;
using ParkingLotSystem.Core.Domain;

namespace ParkingLotSystem.CalculationStrategies
{
    public class TrackedVehicleFareStrategy : IFareStrategy
    {
        public decimal CalculateFare(Ticket ticket, decimal fare)
        {
            // Check if the vehicle is tracked
            if (ticket.Vehicle.IsTracked)
            {
                // Apply a 100% surcharge (double the fare)
                fare += fare * 1.0m;
            }

            return fare;
        }
    }
}
