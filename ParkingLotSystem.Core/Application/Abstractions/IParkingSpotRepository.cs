using ParkingLotSystem.ParkingSpots;

namespace ParkingLotSystem;

public interface IParkingSpotRepository
{
    Task<IReadOnlyCollection<ParkingSpot>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<ParkingSpot?> GetBySpotNumberAsync(int spotNumber, CancellationToken cancellationToken = default);

    Task UpsertAsync(ParkingSpot parkingSpot, CancellationToken cancellationToken = default);

    Task DeleteAsync(int spotNumber, CancellationToken cancellationToken = default);
}
