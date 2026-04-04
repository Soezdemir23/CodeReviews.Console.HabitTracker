// See https://aka.ms/new-console-template for more information

using Microsoft.Data.Sqlite;

var databasePath = Path.Join(Directory.GetCurrentDirectory(), "data");
var databasePathWithDatabase = Path.Join(databasePath, "habitLogger.db");

// create a connection or establish a connection
string connectionString = $"Data Source={databasePathWithDatabase}";

using var connection = new SqliteConnection(connectionString);
try
{
    connection.Open();
    System.Console.WriteLine($"Connection state: {connection.State}"); // shows of it's open
    System.Console.WriteLine("Database is alive.");
}
catch (SqliteException ex)
{
    System.Console.WriteLine($"Failed to connect: {ex.Message}");
}

// run a ping query to test if the database can be interacted with:
var cmd = connection.CreateCommand();
cmd.CommandText = "Select 1";
var result = cmd.ExecuteScalar();

System.Console.WriteLine();

// Clear the Console and then Start the menu:

