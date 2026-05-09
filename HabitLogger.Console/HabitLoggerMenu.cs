using HabitLogger.HabitRepository;
using Spectre.Console;

namespace HabitLogger.HabitLoggerMenu;



/// <summary>
/// The UI the user deals with, it chooses an action and the method calls the other method in the habitloggerrepository. 
/// </summary>

public class HabitLoggerMenu
{

    private readonly HabitLoggerRepository _repo;
    public HabitLoggerMenu(HabitLoggerRepository repo)
    {
        _repo = repo;


    }


    public void MainMenu()
    {
        while (true)
        {


            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>().Title("[bold green]Habit Logger Main Menu[/]")
                .AddChoices(new[]
                {
                "Add habit",
                "View / Edit past logs",
                "Settings",
                "Exit"

                })
            );

            switch (choice)
            {
                case "Add habit": AddHabit(); break;
                case "View / Edit past logs": OpenTable(); break;
                case "Settings": OpenSettings(); break;
                case "Exit": return;

            }
        }
    }


    /// <summary>
    /// If possible, a popup menu.
    /// otherwise create a new submenu 
    /// </summary>
    private void OpenSettings()
    {
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>().Title("[bold green]Settings[/]")
                .AddChoices(new[]
                {
                    "Delete All Entries",
                    "Re-insert Fake Data",
                    "Return to Mainmenu"
                })
            );


            switch (choice)
            {
                case "Delete All Entries": DeleteAllHabitEntries(); break;
                case "Re-insert Fake Data": PopulateWithFakeHabits(); break;
                default: return;
            }
        }

    }
    /// <summary>
    /// I am still thinking about this method and I have no idea if Spectre supports it as best practice.
    /// My hope is to have the following rough idea:
    /// Header: Textfield the user can enter what they are looking for. Tab activates or deactivates it, basically the user can change between the table and the textsearch. the results of the search is show in the table below. Enter
    /// Body: The table with the sql data that displays the entries separated with every 10 entries in a new page. User can navigate the page with the up and down arrow key in the page and browse pages with the page up and page down keys. The user can not cross over to the previous or next page with the arrow keys yet.
    /// Footer: 
    /// Displays what to do once an entry is selected:
    ///     - modify it
    ///         - popup what you want to change with the habit properties separated in each their own:
    ///             - habit name
    ///             - quantity
    ///             - date of habit
    ///         - ask to confirm:
    ///             - yes -> returns back to table with content changed
    ///             - no -> retunrs back to the popup
    ///         - exit back to table with ESC
    ///     - remove it
    ///         - popup if they really want to remove it
    ///             - yes -> removes entry
    ///             - no -> returns back to the selection
    ///     
    /// </summary>
    private void OpenTable()
    {

    }



    public void PopulateWithFakeHabits()
    {
        throw new NotImplementedException();
    }



    /// <summary>
    /// Access all the habits from the repository as a
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void GetAllHabitEntries()
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// Simply remove the Entries in the database, all of them.
    /// Part of the Options in Settings, should also prompt the user if they are sure.
    /// Returns how many rows of habits have been affected.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void DeleteAllHabitEntries()
    {
        throw new NotImplementedException();
    }


    /// <summary>
    /// Simply adding a habit.
    /// Ask the user for name of the habit.
    /// Ask the user fo the date of this habit being expressed.
    /// Ask the user for quantity of the habit.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void AddHabit()
    {
        throw new NotImplementedException();
    }
}