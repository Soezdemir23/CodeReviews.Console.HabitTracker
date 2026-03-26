# Habit Logger

## Requirements

- Build an application to log occurrences of a habit.
- Habits cannot be tracked by time, only by quantity.
- Users must be able to input the date of the habit occurrence.
- The application must store and retrieve data from a real database.
- When the application starts, it should create a SQLite database if one does not already exist.
- It should also create a table in the database where habits are logged.
- Users should be able to insert, delete, update, and view logged habits.
- Handle possible errors so the application does not crash during normal use.
- You can only interact with the database using ADO.NET. Do not use mappers such as Entity Framework or Dapper.
- Follow the DRY principle and avoid code repetition.
- The project must include a README file explaining how the app works and your thought process:
  - What was hard?
  - What was easy?
  - What did you learn?

## Tips

- Learn about the KISS principle and try to apply it to this project.
- Test SQL commands in DB Browser before using them in the program.
- Improve user experience when asking for a date input by giving the option to use today's date.
- You can keep all code in a single class, but try to move toward OOP if possible.
- Do not forget input validation:
  - Check for incorrect dates.
  - Handle invalid menu options.
  - Handle cases where the user inputs a string instead of a number.

## Challenges

- Build the habit of writing unit tests. Integration tests can come later, after you are comfortable with unit tests.
- If you have not already, use parameterized queries to improve security.
- Let users create their own habits to track. This requires allowing them to choose the unit of measurement for each habit.
  - Tip: do not create a separate table for each habit.
- Seed data automatically when the database is created for the first time:
  - Generate a few habits.
  - Insert around 100 records with randomly generated values.
  - This helps during development so you do not have to reinsert data each time.

## Changing Your Working Directory

This setup makes .NET build your project in your main folder. By default, it builds into a `bin` folder. To keep things simple, this helps keep the SQLite database in a predictable place.

This creates a `Properties` folder with a `launchSettings.json` file containing your configuration. This is especially important for SQLite apps, because you want the database file created in the same folder as the application to avoid confusion.

To configure it:

1. Click the chevron next to your app name in the top menu.
2. Click `{nameoftheapp} Debug Properties`.
3. Copy your project directory path into the **Working Directory** field.

To get your project path, you can:

- Right-click the project in Solution Explorer and choose **Copy Full Path**, or
- Find it in your file explorer.

If you are using Mac or Visual Studio Code, reach out and I can explain the equivalent setup.