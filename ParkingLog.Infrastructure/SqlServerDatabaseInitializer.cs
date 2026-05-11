using Microsoft.Data.SqlClient;

namespace ParkingLog.Infrastructure;

public sealed class SqlServerDatabaseInitializer(string connectionString)
{
    public async Task EnsureCreatedAsync(CancellationToken cancellationToken = default)
    {
        await EnsureDatabaseCreatedAsync(cancellationToken);

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand(SqlServerParkingSpotSchema.SchemaSql, connection);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private async Task EnsureDatabaseCreatedAsync(CancellationToken cancellationToken)
    {
        var databaseConnection = new SqlConnectionStringBuilder(connectionString);
        var databaseName = databaseConnection.InitialCatalog;
        if (string.IsNullOrWhiteSpace(databaseName))
        {
            return;
        }

        var masterConnection = new SqlConnectionStringBuilder(connectionString)
        {
            InitialCatalog = "master"
        };

        await using var connection = new SqlConnection(masterConnection.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        const string sql = """
            IF DB_ID(@DatabaseName) IS NULL
            BEGIN
                DECLARE @CreateDatabaseSql nvarchar(max) = N'CREATE DATABASE ' + QUOTENAME(@DatabaseName);
                EXEC(@CreateDatabaseSql);
            END;
            """;

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@DatabaseName", databaseName);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}
