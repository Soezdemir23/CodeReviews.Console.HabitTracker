using Microsoft.Data.Sqlite;

namespace HabitLogger.DatabaseBootstrapper;


public interface IConnectionFactory
{
    SqliteConnection OpenConnection();
    void CloseConnection(SqliteConnection connection);
}


public class DatabaseBootstrapper : IConnectionFactory
{
    private readonly string _connectionString;


    public DatabaseBootstrapper(string databaseFilePath)
    {
        _connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = databaseFilePath,
            Mode = SqliteOpenMode.ReadWriteCreate
        }.ToString();
    }

    public void CloseConnection(SqliteConnection connection)
    {
        connection.Close();
    }

    public SqliteConnection OpenConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }
}