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
                new SelectionPrompt<string>().Title("[bold green]Habit Logger[/]")
                .AddChoices(new[]
                {
                "Add habit",
                "Log habit entry (habit + date + quantity)",
                "View entries (show habit name, date, day-of-week, quantity)",
                "Edit entry",
                "Delete entry",
                "Exit"

                })
            );

            switch (choice)
            {
                case "Add habit": AddHabit(); break;
                case "Log habit entry (habit + date + quantity)": LogHabit(); break;
                case "View entries (show habit name, date, day-of-week, quantity)": ViewEntries(); break;
                case "Edit entry": EditEntry(); break;
                case "Delete entry": DeleteEntry(); break;
                case "Exit": return;

            }
        }
    }

    private void DeleteEntry()
    {
        Console.WriteLine("This method has not been implemented yet (DeleteEntry).");
    }

    private void EditEntry()
    {
        Console.WriteLine("This method has not been implemented yet (EditEntry).");
    }

    private void ViewEntries()
    {
        Console.WriteLine("This method has not been implemented yet (ViewEntries).");
    }

    private void LogHabit()
    {
        Console.WriteLine("This method has not been implemented yet (LogHabit).");
    }

    private void AddHabit()
    {
        Console.WriteLine("This method has not been implemented yet (AddHabit).");
    }
}