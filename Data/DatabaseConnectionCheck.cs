using Microsoft.EntityFrameworkCore;

namespace RoomReservation_Item_I13L.Data;

public static class DatabaseConnectionCheck
{
    public static async Task<ConnectionCheckResult> CheckConnectionAsync(DbContext? dbContext = null)
    {
        var result = new ConnectionCheckResult
        {
            IsConnected = false,
            Message = "No database connection configured",
            Timestamp = DateTime.UtcNow
        };

        if (dbContext == null)
        {
            result.Message = "DbContext is not configured";
            result.CurrentStorageType = "In-Memory Lists";
            result.Note = "Data will be lost when application restarts. Database connection is not configured.";
            return result;
        }

        try
        {
            var canConnect = await dbContext.Database.CanConnectAsync();
            
            if (canConnect)
            {
                result.IsConnected = true;
                result.Message = "Successfully connected to database";
                result.DatabaseName = dbContext.Database.GetDbConnection().Database;
                result.ServerName = dbContext.Database.GetDbConnection().DataSource;
                
                var tables = new List<string>();
                try
                {
                    var sql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
                    var rawSqlResult = await dbContext.Database.SqlQueryRaw<string>(sql).ToListAsync();
                    tables = rawSqlResult;
                }
                catch
                {
                    try
                    {
                        var connection = dbContext.Database.GetDbConnection();
                        await connection.OpenAsync();
                        var entityTypes = dbContext.Model.GetEntityTypes();
                        tables = entityTypes.Select(e => e.GetTableName() ?? e.Name).ToList();
                        await connection.CloseAsync();
                    }
                    catch
                    {
                    }
                }
                
                result.TablesFound = tables;
                result.TableCount = tables.Count;
            }
            else
            {
                result.IsConnected = false;
                result.Message = "Cannot connect to database";
            }
        }
        catch (Exception ex)
        {
            result.IsConnected = false;
            result.Message = $"Database connection failed: {ex.Message}";
            result.ErrorDetails = ex.ToString();
        }

        return result;
    }

    public static ConnectionCheckResult CheckCurrentSystem()
    {
        return new ConnectionCheckResult
        {
            IsConnected = false,
            Message = "System is using in-memory data storage (no database connection)",
            Timestamp = DateTime.UtcNow,
            CurrentStorageType = "In-Memory Lists",
            Note = "Data will be lost when application restarts. Database connection is not configured."
        };
    }
}

public class ConnectionCheckResult
{
    public bool IsConnected { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? DatabaseName { get; set; }
    public string? ServerName { get; set; }
    public List<string> TablesFound { get; set; } = new();
    public int TableCount { get; set; }
    public string? ErrorDetails { get; set; }
    public DateTime Timestamp { get; set; }
    public string? CurrentStorageType { get; set; }
    public string? Note { get; set; }
}

