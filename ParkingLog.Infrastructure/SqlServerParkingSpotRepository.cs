using Microsoft.Data.SqlClient;
using ParkingLotSystem;
using ParkingLotSystem.ParkingSpots;
using ParkingLotSystem.Vehicles;

namespace ParkingLog.Infrastructure;

public sealed class SqlServerParkingSpotRepository(string connectionString) : IParkingSpotRepository
{
    public async Task<IReadOnlyCollection<ParkingSpot>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT SpotNumber, SpotKind, Size, IsVip, Status, AssignedVehicleLicensePlate, AssignedVehicleSize
            FROM dbo.ParkingSpots
            ORDER BY SpotNumber;
            """;

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var spots = new List<ParkingSpot>();
        while (await reader.ReadAsync(cancellationToken))
        {
            spots.Add(ReadParkingSpot(reader));
        }

        return spots;
    }

    public async Task<ParkingSpot?> GetBySpotNumberAsync(int spotNumber, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT SpotNumber, SpotKind, Size, IsVip, Status, AssignedVehicleLicensePlate, AssignedVehicleSize
            FROM dbo.ParkingSpots
            WHERE SpotNumber = @SpotNumber;
            """;

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@SpotNumber", spotNumber);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        return await reader.ReadAsync(cancellationToken) ? ReadParkingSpot(reader) : null;
    }

    public async Task UpsertAsync(ParkingSpot parkingSpot, CancellationToken cancellationToken = default)
    {
        const string sql = """
            MERGE dbo.ParkingSpots WITH (HOLDLOCK) AS Target
            USING (VALUES (
                @SpotNumber,
                @SpotKind,
                @Size,
                @IsVip,
                @Status,
                @AssignedVehicleLicensePlate,
                @AssignedVehicleSize
            )) AS Source (
                SpotNumber,
                SpotKind,
                Size,
                IsVip,
                Status,
                AssignedVehicleLicensePlate,
                AssignedVehicleSize
            )
            ON Target.SpotNumber = Source.SpotNumber
            WHEN MATCHED THEN
                UPDATE SET
                    SpotKind = Source.SpotKind,
                    Size = Source.Size,
                    IsVip = Source.IsVip,
                    Status = Source.Status,
                    AssignedVehicleLicensePlate = Source.AssignedVehicleLicensePlate,
                    AssignedVehicleSize = Source.AssignedVehicleSize
            WHEN NOT MATCHED THEN
                INSERT (SpotNumber, SpotKind, Size, IsVip, Status, AssignedVehicleLicensePlate, AssignedVehicleSize)
                VALUES (
                    Source.SpotNumber,
                    Source.SpotKind,
                    Source.Size,
                    Source.IsVip,
                    Source.Status,
                    Source.AssignedVehicleLicensePlate,
                    Source.AssignedVehicleSize
                );
            """;

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand(sql, connection);
        AddParkingSpotParameters(command, parkingSpot);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task DeleteAsync(int spotNumber, CancellationToken cancellationToken = default)
    {
        const string sql = "DELETE FROM dbo.ParkingSpots WHERE SpotNumber = @SpotNumber;";

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@SpotNumber", spotNumber);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static void AddParkingSpotParameters(SqlCommand command, ParkingSpot parkingSpot)
    {
        command.Parameters.AddWithValue("@SpotNumber", parkingSpot.SpotNumber);
        command.Parameters.AddWithValue("@SpotKind", GetSpotKind(parkingSpot));
        command.Parameters.AddWithValue("@Size", (int)parkingSpot.Size);
        command.Parameters.AddWithValue("@IsVip", parkingSpot.IsVip);
        command.Parameters.AddWithValue("@Status", (int)parkingSpot.Status);
        command.Parameters.AddWithValue("@AssignedVehicleLicensePlate", (object?)parkingSpot.AssignedVehicle?.LicensePlate ?? DBNull.Value);
        command.Parameters.AddWithValue("@AssignedVehicleSize", (object?)(int?)parkingSpot.AssignedVehicle?.Size ?? DBNull.Value);
    }

    private static ParkingSpot ReadParkingSpot(SqlDataReader reader)
    {
        var spotNumber = reader.GetInt32(reader.GetOrdinal("SpotNumber"));
        var spotKind = reader.GetString(reader.GetOrdinal("SpotKind"));
        var size = (VehicleSize)reader.GetInt32(reader.GetOrdinal("Size"));
        var isVip = reader.GetBoolean(reader.GetOrdinal("IsVip"));
        var status = (ParkingSpotStatus)reader.GetInt32(reader.GetOrdinal("Status"));
        var spot = CreateParkingSpot(spotNumber, spotKind, size, isVip);

        switch (status)
        {
            case ParkingSpotStatus.Available:
                break;
            case ParkingSpotStatus.Occupied:
                spot.AssignVehicle(ReadAssignedVehicle(reader));
                break;
            case ParkingSpotStatus.UnderConstruction:
                spot.MarkUnderConstruction();
                break;
            case ParkingSpotStatus.Cleaning:
                spot.MarkCleaning();
                break;
            default:
                throw new DataMappingException($"Unknown parking spot status '{status}'.");
        }

        return spot;
    }

    private static Vehicle ReadAssignedVehicle(SqlDataReader reader)
    {
        var licensePlateOrdinal = reader.GetOrdinal("AssignedVehicleLicensePlate");
        var vehicleSizeOrdinal = reader.GetOrdinal("AssignedVehicleSize");

        if (reader.IsDBNull(licensePlateOrdinal) || reader.IsDBNull(vehicleSizeOrdinal))
        {
            throw new DataMappingException("Occupied parking spots must have an assigned vehicle.");
        }

        var licensePlate = reader.GetString(licensePlateOrdinal);
        var vehicleSize = (VehicleSize)reader.GetInt32(vehicleSizeOrdinal);

        return vehicleSize switch
        {
            VehicleSize.Small => new Motorcycle(licensePlate),
            VehicleSize.Medium => new Car(licensePlate),
            VehicleSize.Large => new Truck(licensePlate),
            _ => throw new DataMappingException($"Unknown vehicle size '{vehicleSize}'.")
        };
    }

    private static ParkingSpot CreateParkingSpot(int spotNumber, string spotKind, VehicleSize size, bool isVip)
    {
        return spotKind switch
        {
            nameof(CompactSpot) => new CompactSpot(spotNumber, isVip),
            nameof(RegularSpot) => new RegularSpot(spotNumber, isVip),
            nameof(OversizedSpot) => new OversizedSpot(spotNumber, isVip),
            nameof(HandicappedSpot) => new HandicappedSpot(spotNumber, isVip),
            _ => size switch
            {
                VehicleSize.Small => new CompactSpot(spotNumber, isVip),
                VehicleSize.Medium => new RegularSpot(spotNumber, isVip),
                VehicleSize.Large => new OversizedSpot(spotNumber, isVip),
                _ => throw new DataMappingException($"Unknown parking spot size '{size}'.")
            }
        };
    }

    private static string GetSpotKind(ParkingSpot parkingSpot)
    {
        return parkingSpot switch
        {
            CompactSpot => nameof(CompactSpot),
            RegularSpot => nameof(RegularSpot),
            OversizedSpot => nameof(OversizedSpot),
            HandicappedSpot => nameof(HandicappedSpot),
            _ => parkingSpot.GetType().Name
        };
    }
}
