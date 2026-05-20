using Microsoft.Data.Sqlite;

namespace ApiRefactor.Models;

public class SqlConnection
{
    private static string ConnectionString = "Data Source=App_Data/waves.db";

    public static SqliteConnection GetSqlConnection()
    {
        return new SqliteConnection(ConnectionString);
    }
}
