namespace ParkingLog.Infrastructure;

public static class SqlServerParkingSpotSchema
{
    public const string SchemaSql = """
        IF SCHEMA_ID(N'dbo') IS NULL
            EXEC(N'CREATE SCHEMA dbo');

        IF OBJECT_ID(N'dbo.ParkingSpots', N'U') IS NULL
        BEGIN
            CREATE TABLE dbo.ParkingSpots
            (
                SpotNumber int NOT NULL,
                SpotKind nvarchar(32) NOT NULL,
                Size int NOT NULL,
                IsVip bit NOT NULL CONSTRAINT DF_ParkingSpots_IsVip DEFAULT 0,
                Status int NOT NULL CONSTRAINT DF_ParkingSpots_Status DEFAULT 0,
                AssignedVehicleLicensePlate nvarchar(32) NULL,
                AssignedVehicleSize int NULL,
                CreatedAtUtc datetime2(7) NOT NULL CONSTRAINT DF_ParkingSpots_CreatedAtUtc DEFAULT SYSUTCDATETIME(),
                UpdatedAtUtc datetime2(7) NOT NULL CONSTRAINT DF_ParkingSpots_UpdatedAtUtc DEFAULT SYSUTCDATETIME(),
                RowVersion rowversion NOT NULL,

                CONSTRAINT PK_ParkingSpots PRIMARY KEY CLUSTERED (SpotNumber),
                CONSTRAINT CK_ParkingSpots_Size CHECK (Size IN (1, 2, 3)),
                CONSTRAINT CK_ParkingSpots_Status CHECK (Status IN (0, 1, 2, 3)),
                CONSTRAINT CK_ParkingSpots_AssignedVehicleSize CHECK (AssignedVehicleSize IS NULL OR AssignedVehicleSize IN (1, 2, 3)),
                CONSTRAINT CK_ParkingSpots_OccupiedVehicle CHECK
                (
                    (Status = 1 AND AssignedVehicleLicensePlate IS NOT NULL AND AssignedVehicleSize IS NOT NULL)
                    OR
                    (Status <> 1 AND AssignedVehicleLicensePlate IS NULL AND AssignedVehicleSize IS NULL)
                )
            );
        END;

        IF OBJECT_ID(N'dbo.TR_ParkingSpots_SetUpdatedAtUtc', N'TR') IS NULL
            EXEC(N'
                CREATE TRIGGER dbo.TR_ParkingSpots_SetUpdatedAtUtc
                ON dbo.ParkingSpots
                AFTER UPDATE
                AS
                BEGIN
                    SET NOCOUNT ON;

                    UPDATE ParkingSpots
                    SET UpdatedAtUtc = SYSUTCDATETIME()
                    FROM dbo.ParkingSpots ParkingSpots
                    INNER JOIN inserted ON inserted.SpotNumber = ParkingSpots.SpotNumber;
                END;
            ');
        """;
}
