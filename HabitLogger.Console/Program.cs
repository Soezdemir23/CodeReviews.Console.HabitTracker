// See https://aka.ms/new-console-template for more information

var databasePath = Path.Join(Directory.GetCurrentDirectory(), "data");
var databasePathWithDatabase = Path.Join(databasePath, "habitLogger.db");

// create a connection or establish a connection
string connectionString = $"Data Source={databasePath}";

// a Test to see if I can query anything at all:

