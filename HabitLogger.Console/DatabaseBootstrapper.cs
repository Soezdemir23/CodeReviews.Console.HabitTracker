using Microsoft.Data.Sqlite;

namespace HabitLogger.DatabaseBootstrapper;


public interface IConnectionFactory
{
    SqliteConnection OpenConnection();
    void CloseConnection(SqliteConnection connection);
}

/// <summary>
/// Class that should create and manage the database actions and takes orders from the HabitLoggerRepository
/// </summary>
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

    public void InitDatabase()
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();

        command.CommandText = """
            Create table if not exists Habits(
                Id          Integer primary key autoincrement,
                Name        Text not null check(length(Name) <=50),
                CreatedAt   Text not null Default (datetime('now', 'localtime')),
                Quantity    Integer not null check(Quantity >= 0)
            );
        """;

        command.ExecuteNonQuery();

    }
}