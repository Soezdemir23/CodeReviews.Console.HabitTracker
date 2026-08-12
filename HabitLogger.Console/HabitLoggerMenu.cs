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
        string keywords = String.Empty;

        var table = new Table().BorderColor(Color.Gold3).Title("[magenta]Your logged habits[/]");

        /*         // Columns:
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
         */
        table.ShowFooters();

        AnsiConsole.Write(table);

        //Below this line we are implementing code that needs to be refactored later 
        //#first:Reduce the table to ten entries for now. implement number of pages.
        //Then implement navigation.

        int currentPage = 0; // in string when shown, it needs to be shown as 1 at start;
        // get the correct number of pages after dividing by ten entries
        int maxPages = habits.Count % 10 != 0 ? habits.Count / 10 + 1 : habits.Count / 10;

        while (true)
        {
            Console.WriteLine($"Page: {currentPage}\nmaxPages: {maxPages}");
            // takes the habits list, current page, the maximum pages, renders it here
            // curentPage can change, but we have a problem with the rest.
            var currentPageEntries = RenderPage(habits, currentPage, maxPages).ToList();

            //prompt the user to select next page, previous page, exit
            //Aw sheeeeit, pagination kinda works. get the currentPageEntries, ask the user which Habit it wants changed (by number),
            //get the id, modify the habit, push it into the database.
            //should work, right?
            if (currentPage == 0)//first page, no previous page
            {
                var action = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("[green] Choose your next actions DEBUG:1.[/]")
                    .AddChoices(
                        "Next page",
                        "Modify Habit (by row in page)",
                        "Return to previous menu")
                );

                switch (action)
                {
                    case "Next page":
                        currentPage++;
                        break;
                    case "Modify Habit (by row in page)":
                        ModifyHabitByRowInPage(currentPageEntries, habits);
                        break;
                    default:
                        return;
                }
            }
            else if (currentPage == maxPages)
            {
                var action = AnsiConsole.Prompt(
                   new SelectionPrompt<string>()
                   .Title("[green] Choose your next actions DEBUG: 2.[/]")
                   .AddChoices(
                       "Previous page",
                       "Modify Habit (by row in page)",
                       "return to menu")
               );

                switch (action)
                {
                    case "Previous page":
                        currentPage--;
                        break;

                    case "Modify Habit (by row in page)":
                        ModifyHabitByRowInPage(currentPageEntries, habits);
                        break;
                    default:
                        return;
                }
            }
            else
            {
                var action = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("[green] Choose your next actions DEBUG: 3.[/]")
                    .AddChoices(
                        "Next page",
                        "Previous page",
                        "Modify Habit (by row in page)",
                        "return to menu")
                );

                switch (action)
                {
                    case "Next page":
                        currentPage++;
                        break;
                    case "Previous page":
                        currentPage--;
                        break;
                    case "Modify Habit (by row in page)":
                        ModifyHabitByRowInPage(currentPageEntries, habits);
                        break;
                    default:
                        return;
                }
            }
        }
    }


    /// <summary>
    /// modifies rows in page.Takes ID in currentPageEntries and updates the database with the new habit changes.
    /// Then also updates the habits list. This way we do not have to refetch all habits from the database every single time.
    /// </summary>
    /// <param name="currentPageEntries"></param>
    /// <param name="habits"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void ModifyHabitByRowInPage(List<Habit> currentPageEntries, List<Habit> habits)
    {
        //prompt the user for which row it wants to change
        var row = AnsiConsole.Prompt(
            new TextPrompt<int>($"Pick row to modify (1-{currentPageEntries.Count}):")
            .Validate(n =>
                n >= 1 && n <= currentPageEntries.Count
                ? ValidationResult.Success()
                : ValidationResult.Error("[red]Row out of range.[/]")
                )
        );

        var selectedHabit = currentPageEntries[row - 1];

        var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("[bold green]Which parts of the habit do you want to change?[/]")
            .AddChoices<string>(
                "Name", "Quantity", "Date", "Cancel"
            )
        );

        // Handle chose option
        switch (option)
        {
            case "Name":
                string newHabitName = AnsiConsole.Ask<string>("[gold3]What is the new habit for this entry? [/]");
                selectedHabit.HabitName = newHabitName;
                break;
            case "Quantity":
                int newHabitQuantity = AnsiConsole.Ask<int>("[gold3]What is the new quantity for this behavior?[/]");
                selectedHabit.HabitQuantity = newHabitQuantity;
                break;
            case "Date":
                var dateText = AnsiConsole.Prompt(
                    new TextPrompt<string>("Enter date [gold3](yyyy-MM-dd)[/]")
                    .Validate(d => HabitInputValidator.TryParseHabitDate(d, out _)
                    ? ValidationResult.Success()
                    : ValidationResult.Error("[red]Invalid date format. Use yyyy-MM-dd.[/]"))
                );

                if (DateTime.TryParseExact(
                    dateText,
                    "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out var parsedDate
                ))
                {
                    AnsiConsole.MarkupLine("[red]Could not parse the date.([/])");
                    break;
                }

                selectedHabit.CreatedAt = parsedDate;
                break;
            case "Cancel":
            default:
                return;
        }

        // Confirm things
        var confirmation = AnsiConsole.Confirm($"Does this look good to you?\nHabitname:{selectedHabit.HabitName}\nHabit quantity:{selectedHabit.HabitQuantity}\nHabit date:{selectedHabit.CreatedAt.ToString("yyyy-MM-dd")}");

        if (!confirmation)
        {
            AnsiConsole.MarkupLine("[yellow]Update Cancelled[/]");
            return;
        }

        _repo.UpdateHabit(selectedHabit);
        AnsiConsole.MarkupLine("[green]Behavior has been changed.[/]");
    }




    /// <summary>
    /// Renders the table with pagination.
    /// </summary>
    /// <param name="habits"></param>
    /// <param name="currentPage"></param>
    /// <param name="maxPages"></param>
    /// 
    private IEnumerable<Habit> RenderPage(List<Habit> habits, int currentPage, double maxPages)
    {
        var table = new Table().BorderColor(Color.Gold3).Title("[magenta]Your logged habits[/]");

        // Columns:
        table.AddColumn("Habit number");
        table.AddColumn("Habit name");
        table.AddColumn("Habit quantity");
        table.AddColumn("Habit date");


        // calculate which entries are required to be rendered at the moment:
        // let's say we have 777 entries and max 10 entries:
        // 77.7 pages it returns, so 78.
        // if it's not the last page, we can assume it is always a page of 10 entries.
        // we should isolate them as a list and then simply render them below

        var currentPageEntries = habits.Skip(currentPage * 10).Take(10);
        var entryNumber = 0;
        foreach (var habit in currentPageEntries)
        {
            entryNumber++;
            table.AddRow(
            entryNumber.ToString(),
            habit.HabitName,
            habit.HabitQuantity.ToString(),
            habit.CreatedAt.ToString()
            );

        }
        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine($"Page {currentPage} of {maxPages}"); //nice debugging and information for users

        return currentPageEntries;
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
