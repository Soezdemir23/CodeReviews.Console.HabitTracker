# Habit logger

## Requirements:

- application to log occurences of a habit
- can't be tracke by time, only quantity
- Users need to be able to input the date of the occurence of the habit
- The application should store and retrieve data from a real database
- When the application starts, it should create a sqlite database, if one is not present.
-  it should also create a table in the database, where the habit will be logged
- the users should be able to insert, delete, update and view the logged habit.
- You should handle all possible errors so that the application never crashes.
- You can only interact with the database using ADO.NET. You can't use mappers such as Entity Framework or Dapper.
- Follow the DRY principle and avoid code repetition
- Your project needs to containe Readme file where you'll explain how your app works and tell a little bit about your thought progress.
  - What was hard?
  - What was easy?
  - Waht have you learned?

## Tips:

- Learn about the kiss principle and try to apply it to this project
- Test the sql commands on the db browser before using them in the program.
- improve user's experience when asking for a date input, give them the option to add today's date
- You can keep all the code in a single class, but try to pivot towards oop if you can
- Don't forget the user input's validation: Check for incorrect dates. What happens if a menu option is chosen that's not available? What happens if the users input a string instead of a number? 

## Challenges:
- Get into the habit of unit tests, integration tests are for when you get used to writing unit tests.
- if you haven't, get into trying to use parameterized queries to make application more secure
- Let the users create their won habits to track. that will require that you let them choose the unit of ,easurement of each habit. Hot tip: You should not createaa a table for each habit.
- Seed Data into the database automatically when the database gets created for the first time, generating a few habits and inserting a hundred records with randomly generated values. this is specially helpful during dev3elopment so you don't have to reinsert data every time you create the database

# Changing you working directory

This way .NET will build your project in your main folder. By default it builds your project in a bin folder and just to keep things simple we want to avoid that. That will create a Properties folder with a launchsettings.json file containing your configuration information. This is an important step only for applications that use Sqlite because you want the database to be created in the same folder of the application to avoid confusion.

For that, click on the chevron next to the name of your app on the top menu, click on {nameoftheapp} Debug Properties and copy the path of your directory to the 'Working Directory' field. To find out what your path is, you can right click on your project in the Solution Explorer and on “Copy Full Path” or look it up in your Files Explorer. If you’re using Mac/Visual Studio Code, reach out and I’ll tell you how to do it.