// See https://aka.ms/new-console-template for more information

using HabitLogger.DatabaseBootstrapper;
using HabitLogger.HabitLoggerMenu;
using HabitLogger.HabitRepository;


// define the path where the database is.
// BUG: Opening the project in another folder gives a different path.
// Solved:  Use AppContext.BaseDirectory, which moves the database into the app's actual runtime folder permanently.
//          Also Make sure the data folder exists before sqlite tries to open the db.
var appDirectory = AppContext.BaseDirectory;
var databasePath = Path.Combine(appDirectory, "data");
Directory.CreateDirectory(databasePath);
var databasePathWithDatabase = Path.Combine(databasePath, "habitLogger.db");

// instantiate the objects and integrate them in that order
DatabaseBootstrapper strap = new(databaseFilePath: databasePathWithDatabase);
HabitLoggerRepository repository = new HabitLoggerRepository(strap);
HabitLoggerMenu loggerMenu = new HabitLoggerMenu(repository);

//run the initialization of the database by adding the habits table if it doesn't exist:
strap.InitDatabase();

loggerMenu.MainMenu();



