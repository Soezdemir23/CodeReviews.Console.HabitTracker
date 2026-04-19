// See https://aka.ms/new-console-template for more information

using HabitLogger.DatabaseBootstrapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;

var databasePath = Path.Join(Directory.GetCurrentDirectory(), "data");
var databasePathWithDatabase = Path.Join(databasePath, "habitLogger.db");

// // create a connection or establish a connection
// string connectionString = $"Data Source={databasePathWithDatabase}";

// using var connection = new SqliteConnection(connectionString);
// try
// {
//     connection.Open();
//     System.Console.WriteLine($"Connection state: {connection.State}"); // shows of it's open
//     System.Console.WriteLine("Database is alive.");
// }
// catch (SqliteException ex)
// {
//     System.Console.WriteLine($"Failed to connect: {ex.Message}");
// }

// // run a ping query to test if the database can be interacted with:
// var cmd = connection.CreateCommand();
// cmd.CommandText = "Select 1";
// var result = cmd.ExecuteScalar();

// System.Console.WriteLine();




DatabaseBootstrapper strap = new(databaseFilePath: databasePathWithDatabase);
strap.OpenConnection();
Console.WriteLine("written");

// Clear the Console and then Start the menu:



// var choice = AnsiConsole.Prompt(
//     new SelectionPrompt<string>().Title("[bold green]Habit Logger[/]")
//     .AddChoices(new[]
//     {
//         "Add habit",
//         "Log habit entry (habit + date + quantity)",
//         "View entries (show habit name, date, day-of-week, quantity)",
//         "Edit entry",
//         "Delete entry",
//         "Exit"

//     })
// );

// switch (choice)
// {
//     case "Add habit": AddHabit(); break;
//     case "Log habit entry (habit + date + quantity)": LogHabit(); break;
//     case "View entries (show habit name, date, day-of-week, quantity)": ViewEntries(); break;
//     case "Edit entry": EditEntry(); break;
//     case "Delete entry": DeleteEntry(); break;
//     case "Exit": return;

// }

// void DeleteEntry()
// {
//     throw new NotImplementedException();
// }

// void EditEntry()
// {
//     throw new NotImplementedException();
// }

// void ViewEntries()
// {
//     throw new NotImplementedException();
// }

// void LogHabit()
// {
//     throw new NotImplementedException();
// }

// void AddHabit()
// {
//     throw new NotImplementedException();
// }

