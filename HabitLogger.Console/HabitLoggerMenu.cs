using HabitLogger.HabitRepository;
using HabitLogger.Models;
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
                new SelectionPrompt<string>()
                    .Title("[bold green]Habit Logger Main Menu[/]")
                    .AddChoices(new[] { "Add habit", "View / Edit past logs", "Settings", "Exit" })
            );

            switch (choice)
            {
                case "Add habit":
                    AddHabit();
                    break;
                case "View / Edit past logs":
                    OpenTable();
                    break;
                case "Settings":
                    OpenSettings();
                    break;
                case "Exit":
                    return;
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
                new SelectionPrompt<string>()
                    .Title("[bold green]Settings[/]")
                    .AddChoices(
                        new[] { "Delete All Entries", "Re-insert Fake Data", "Return to Mainmenu" }
                    )
            );

            switch (choice)
            {
                case "Delete All Entries":
                    DeleteAllHabitEntries();
                    break;
                case "Re-insert Fake Data":
                    PopulateWithFakeHabits();
                    break;
                default:
                    return;
            }
        }
    }

    /// <summary>
    /// I am still thinking about this method and I have no idea if Spectre supports it as best practice.
    /// My hope is to have the following rough idea:
    /// Header: Textfield the user can enter what they are looking for.
    ///         Tab activates or deactivates it, basically the user can change between the table and the textsearch.
    ///         The results of the search is show in the table below.
    /// Body: The table with the sql data that displays the entries separated with every 10 entries in a new page.
    ///       User can navigate the page with the up and down arrow key in the page and browse pages with the page up and page down keys.
    ///       The user can not cross over to the previous or next page with the arrow keys yet.
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
    /// </summary>
    private void OpenTable()
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold green]Habits[/]")
                .AddChoices(
                    new[] { "Add new Habit", "View Habits", "Modify Habits", "Return to main menu" }
                )
        );
        switch (choice)
        {
            case "Add new Habit":
                AddHabit();
                break;
            case "Modify Habits":
                ModifyHabits();
                break;
            case "View Habits":
                ViewHabits();
                break;
            default:
                return;
        }
    }

    private void ViewHabits()
    {
        var habitEntries = _repo.GetHabits();

        var table = new Table().BorderColor(Color.Gold3).Title("[magenta]Your logged habits[/]");

        // Columns:
        table.AddColumn("Habit id");
        table.AddColumn("Habit name");
        table.AddColumn("Habit quantity");
        table.AddColumn("Habit date");

        foreach (var habit in habitEntries)
        {
            table.AddRow(
                habit.HabitId.ToString(),
                habit.HabitName,
                habit.HabitQuantity.ToString(),
                habit.CreatedAt.ToString()
            );
        }
        AnsiConsole.Write(table);
    }

    private void ModifyHabits()
    {
        var habits = _repo.GetHabits();
        int totalPages = habits.Count / 10; // ten entries per page, what about pages that have less than ten entries? modulo
        int currentPage = 1; // lists are zero count based, except when counted

        string keywords = String.Empty;

        var table = new Table().BorderColor(Color.Gold3).Title("[magenta]Your logged habits[/]");

        // Columns:
        table.AddColumn("Habit id");
        table.AddColumn("Habit name");
        table.AddColumn("Habit quantity");
        table.AddColumn("Habit date");

        // rows, should be ten entries per page (later).
        foreach (var habit in habits)
        {
            table.AddRow(
                habit.HabitId.ToString(),
                habit.HabitName,
                habit.HabitQuantity.ToString(),
                habit.CreatedAt.ToString()
            );
        }

        table.ShowFooters();

        AnsiConsole.Write(table);

        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]Actions[/]")
                    .AddChoices(
                        new[]
                        {
                            "next page",
                            "previous page",
                            //        "search",
                            //      "select by id",
                            "back",
                        }
                    )
            );
            switch (choice)
            {
                case "next page":
                    RenderNextPage();
                    break;
                case "previous page":
                    RenderPreviousPage();
                    break;
                //case "search": FilterByHabits(); break;
                //case "select by id": SelectById(); break;
                default:
                    return;
            }

            // three ways this can go for next page:
            // - next page doesn't exist, the entries aren't enough to support pagionation at the moment
            // - the user wants to try to go further than the totalPages
            // - the user just wants the next available page
            // - edge case: there are entries that can only be seen via modulo

            //there are more pages than totalPages
            if (totalPages > currentPage)
            {
                //first increment the page
                currentPage += 1;
                //then calculate where the next table starts
                int startingCount = currentPage * 10;

                table = new Table()
                    .BorderColor(Color.Gold3)
                    .Title("[magenta]Your logged habits[/]");

                // Columns:
                table.AddColumn("Habit id");
                table.AddColumn("Habit name");
                table.AddColumn("Habit quantity");
                table.AddColumn("Habit date");

                //now get the next list of entries
                //
                for (var i = startingCount; i < startingCount + 10; i++)
                {
                    var habit = habits.ElementAt(i);
                    table.AddRow(
                        habit.HabitId.ToString(),
                        habit.HabitName,
                        habit.HabitQuantity.ToString(),
                        habit.CreatedAt.ToString()
                    );
                }
            }
            else if (totalPages % 10 > 0) // always the last page or the very first one
            {
                //first of all, get the number by modulo
                int rest = totalPages % 10;
                // then get the count like this, zero-count! don't forget
                int startingPoint = habits.Count - rest + 1;

                table = new Table()
                    .BorderColor(Color.Gold3)
                    .Title("[magenta]Your logged habits[/]");

                // Columns:
                table.AddColumn("Habit id");
                table.AddColumn("Habit name");
                table.AddColumn("Habit quantity");
                table.AddColumn("Habit date");

                //now get the next list of entries
                //
                for (var i = startingPoint; i < habits.Count; i++)
                {
                    var habit = habits.ElementAt(i);
                    table.AddRow(
                        habit.HabitId.ToString(),
                        habit.HabitName,
                        habit.HabitQuantity.ToString(),
                        habit.CreatedAt.ToString()
                    );
                }
            }
            else
            {
                AnsiConsole.Write("[red]There is no page after this.[/]");
            }

            //then there is the previous page, which is probably the reverse:
            // - the user can't browse below 1, or in zero count base, below zero... subzero pffft...
            // - of course, what happens if the user is on the first page and not enough entries are there to fill page? nothing
            // - I wonder if there are any other edge cases like in the next page, aside from going subzero....

            if (currentPage < 1)
            {
                AnsiConsole.Write(
                    "[red]You can't go to zero pages or below. If you want subzero go play mortal kombat"
                );
            }
            else
            {
                table = new Table()
                    .BorderColor(Color.Gold3)
                    .Title("[magenta]Your logged habits[/]");

                // Columns:
                table.AddColumn("Habit id");
                table.AddColumn("Habit name");
                table.AddColumn("Habit quantity");
                table.AddColumn("Habit date");

                // rows, should be ten entries per page (later).
                foreach (var habit in habits)
                {
                    table.AddRow(
                        habit.HabitId.ToString(),
                        habit.HabitName,
                        habit.HabitQuantity.ToString(),
                        habit.CreatedAt.ToString()
                    );
                }
            }
        }
    }

    private void RenderPreviousPage()
    {
        throw new NotImplementedException();
    }

    private void RenderNextPage()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// This method simply generates and inserts fake entries, future implementation will check the database or entries list for existing elements
    /// </summary>
    public void PopulateWithFakeHabits()
    {
        // ask the user how many fake entries it needs, make sure it's for now no more than 1000 or less than 1
        var count = AnsiConsole.Prompt(
            new TextPrompt<int>("[grey]How many fake entries do you need? [/]").Validate(n =>
                (n >= 1 && n < 1000)
                    ? ValidationResult.Success()
                    : ValidationResult.Error("[red]Enter a number from 1 to 1000.[/]")
            )
        );

        var confirmed = AnsiConsole.Confirm($"Generating [cyan]{count}[/] fake habits?");
        if (!confirmed)
        {
            AnsiConsole.MarkupLine("[yellow]Cancelled.[/]");
            return;
        }

        var rng = new Random();
        var habitNames = new[]
        {
            "Push-ups completed",
            "Sit-ups completed",
            "Squats done",
            "Jumping jacks performed",
            "Lunges completed",
            "Burpees done",
            "Pull-ups completed",
            "Planks held",
            "Steps walked",
            "Laps swum",
            "Gym reps completed",
            "Workout sets finished",
            "Miles run",
            "Flights of stairs climbed",
            "Mountain climbers done",
            "Yoga sessions practiced",
            "Emails sent",
            "Emails answered",
            "Phone calls made",
            "Meetings attended",
            "Tasks completed",
            "To-do items checked off",
            "Code commits pushed",
            "Bugs fixed",
            "Code reviews completed",
            "Documents drafted",
            "Forms filed",
            "Presentations given",
            "Follow-ups sent",
            "Deadlines met",
            "Pages read",
            "Books finished",
            "Words written",
            "Articles published",
            "Math problems solved",
            "Flashcards reviewed",
            "Vocabulary words learned",
            "Lessons completed",
            "Questions answered",
            "Sketches drawn",
            "Songs practiced",
            "Notes taken",
            "Tutorials watched",
            "Quizzes passed",
            "Dishes washed",
            "Loads of laundry done",
            "Rooms cleaned",
            "Meals cooked",
            "Errands run",
            "Plants watered",
        };

        for (int i = 0; i < count; i++)
        {
            var habitName = habitNames[rng.Next(habitNames.Length)];
            var quantity = rng.Next(1, 21); // 1..20
            var date = DateTime.Now.Date.AddDays(-rng.Next(0, 180)); // last 180 days

            _repo.Addhabit(habitName, quantity, date.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        AnsiConsole.MarkupLine($"[green]Inserted {count} fake entries.[/]");
    }

    /// <summary>
    /// Access all the habits from the repository as a
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private List<Habit> GetAllHabitEntries()
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
        // first get all the entries, so I can ask the user if they really want to delete N rows.

        var choices = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"Do you really want to remove every habit in the database?")
                .AddChoices("Yes", "No")
        );

        if (choices.Equals("Yes"))
        {
            AnsiConsole.MarkupLine("Deleting the database content...");
            int numberOfRows = _repo.RemoveAllHabitEntries();
            AnsiConsole.MarkupLine($"The {numberOfRows} were deleted.");
        }
        else
        {
            AnsiConsole.MarkupLine("Returning back to previous menu");
        }
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
        var name = AnsiConsole.Ask<String>(
            "What habit will you log today? Enter \"quit\" to abort"
        );

        if (name.Equals("quit"))
        {
            return;
        }

        var quantity = AnsiConsole.Prompt(
            new TextPrompt<int>("How many times did you do the habit at that day?")
            // This and the other Validate chain are great candidates for a unit test
            .Validate(q =>
                HabitInputValidator.IsValidQuantity(q)
                    ? ValidationResult.Success()
                    : ValidationResult.Error(
                        "[red]The number of habits on that day must be greater than 0 or smaller than 9999.[/]"
                    )
            )
        );

        var dateChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("When did you do this habit?")
                .AddChoices("Today", "Enter a custom date")
        );

        DateTime date;

        if (dateChoice == "Today")
        {
            date = DateTime.Now;
        }
        else
        {
            var dateText = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter date [grey](yyyy-MM-dd)[/]:")
                // this is also a no brainer candidate for a unit test
                .Validate(d =>
                    HabitInputValidator.TryParseHabitDate(d, out _)
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]Invalid date format. Use yyyy-MM-dd.[/]")
                )
            );
            // This block is also a unit testing candidate maybe?
            DateTime.TryParseExact(
                dateText,
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out date
            );
        }

        AnsiConsole.MarkupLine(
            $"\nHabit: [cyan]{name}[/] | Quantity: [cyan]{quantity}[/] | Date: [cyan]{date:yyyy-MM-dd}[/]"
        );

        var confirmed = AnsiConsole.Confirm("Save this habit?");

        if (!confirmed)
        {
            AnsiConsole.MarkupLine("[yellow]Habit not saved.[/]");
            return;
        }

        _repo.Addhabit(name, quantity, date.ToString("yyyy-MM-dd HH:mm:ss"));
        AnsiConsole.MarkupLine("[green]Habit saved successfully[/]");
    }
}
