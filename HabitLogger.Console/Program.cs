// See https://aka.ms/new-console-template for more information

using HabitLogger.DatabaseBootstrapper;
using HabitLogger.HabitLoggerMenu;


var databasePath = Path.Join(Directory.GetCurrentDirectory(), "data");
var databasePathWithDatabase = Path.Join(databasePath, "habitLogger.db");

// create the database or connect to it
DatabaseBootstrapper strap = new(databaseFilePath: databasePathWithDatabase);
strap.OpenConnection();

//start the menu, keep it on a loop, probably inside the constructor
HabitLoggerMenu menu = new();
menu.MainMenu();

