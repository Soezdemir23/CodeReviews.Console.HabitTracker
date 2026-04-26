// See https://aka.ms/new-console-template for more information

using HabitLogger.DatabaseBootstrapper;
using HabitLogger.HabitLoggerMenu;
using HabitLogger.HabitRepository;


// define the path where the database is.
var databasePath = Path.Join(Directory.GetCurrentDirectory(), "data");
var databasePathWithDatabase = Path.Join(databasePath, "habitLogger.db");

// instantiate the objects and integrate them in that order
DatabaseBootstrapper strap = new(databaseFilePath: databasePathWithDatabase);
HabitLoggerRepository repository = new HabitLoggerRepository(strap);
HabitLoggerMenu loggerMenu = new HabitLoggerMenu(repository);

//run the initialization of the database by adding the habits table if it doesn't exist:
strap.InitDatabase();

loggerMenu.MainMenu();



