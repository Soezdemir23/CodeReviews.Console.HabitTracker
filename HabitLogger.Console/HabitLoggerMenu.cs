using Spectre.Console;

namespace HabitLogger.HabitLoggerMenu;

public class HabitLoggerMenu
{


    public HabitLoggerMenu()
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

    void DeleteEntry()
    {
        throw new NotImplementedException();
    }

    void EditEntry()
    {
        throw new NotImplementedException();
    }

    void ViewEntries()
    {
        throw new NotImplementedException();
    }

    void LogHabit()
    {
        throw new NotImplementedException();
    }

    void AddHabit()
    {
        throw new NotImplementedException();
    }
}