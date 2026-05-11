using Microsoft.Data.SqlClient;

namespace ParkingLog.Infrastructure;

public static class SqlServerConnectionStrings
{
    public static string LocalDb(string databaseName = "ParkingLot")
    {
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = @"(localdb)\MSSQLLocalDB",
            InitialCatalog = databaseName,
            IntegratedSecurity = true,
            TrustServerCertificate = true
        };

        return builder.ConnectionString;
    }

    public static string SqlServer(
        string dataSource,
        string databaseName,
        bool integratedSecurity = true,
        string? userId = null,
        string? password = null,
        bool trustServerCertificate = true)
    {
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = dataSource,
            InitialCatalog = databaseName,
            IntegratedSecurity = integratedSecurity,
            TrustServerCertificate = trustServerCertificate
        };

        if (!integratedSecurity)
        {
            builder.UserID = userId;
            builder.Password = password;
        }

        return builder.ConnectionString;
    }
}
