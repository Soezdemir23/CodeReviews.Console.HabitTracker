using System.Data.SqlTypes;
using HabitLogger.DatabaseBootstrapper;
using HabitLogger.Models;

namespace HabitLogger.HabitRepository;

/// <summary>
/// This class is the bridge between the actions in the HabitLoggerMenu and the DatabaseBootstrapper.
/// User runs a method that calls CRUD Actions here on the DatabaseBootstrapper.
/// </summary>
public class HabitLoggerRepository
{
    private readonly IConnectionFactory _factory;

    public HabitLoggerRepository(IConnectionFactory factory)
    {
        _factory = factory;
    }


    public void Addhabit(string habitName, int habitQuantity, string habitDate)
    {
        using var connection = _factory.OpenConnection();
        var command = connection.CreateCommand();

        command.CommandText = $"""
        Insert into Habits (Name, Quantity, CreatedAt)
        Values (
            $name,
            $quantity,
            $createdAt
        );
        """;


        command.Parameters.AddWithValue("$name", habitName);
        command.Parameters.AddWithValue("$quantity", habitQuantity);
        command.Parameters.AddWithValue("$createdAt", habitDate);

        command.ExecuteNonQuery();
    }

    public void RemoveHabit(int id)
    {
        using var connection = _factory.OpenConnection();
        var command = connection.CreateCommand();

        command.CommandText = $"""
            Delete from Habits
            Where Habits.Id = $Id;
        
        """;

        command.Parameters.AddWithValue("$Id", id);
        command.ExecuteNonQuery();
    }


    /// <summary>
    /// Should return a list of habits, all habits
    /// </summary>
    public List<Habit> GetHabits()
    {
        using var connection = _factory.OpenConnection();
        var command = connection.CreateCommand();

        command.CommandText = $"""
           Select * from Habits;
        """;

        using var reader = command.ExecuteReader();
        List<Habit> habits = new();

        var idOrdinal = reader.GetOrdinal("Id");
        var nameOrdinal = reader.GetOrdinal("Name");
        var quantityOrdinal = reader.GetOrdinal("Quantity");
        var createdAtOrdinal = reader.GetOrdinal("CreatedAt");
        int row = 0;
        while (reader.Read())
        {
            try
            {
                var habit = new Habit
                {
                    HabitId = reader.GetInt32(idOrdinal),
                    HabitName = reader.GetString(nameOrdinal),
                    HabitQuantity = reader.GetInt32(quantityOrdinal),
                    CreatedAt = DateTime.Parse(reader.GetString(createdAtOrdinal))
                };

                habits.Add(habit);
                row++;
            }
            catch (System.Exception exc)
            {
                Console.WriteLine($"Row {row} conversion caused the following error");
                Console.WriteLine($"{exc.Message}");
                Console.WriteLine($"{exc.StackTrace}");

            }
        }

        return habits;
    }

    /// <summary>
    /// The Viewentry
    /// </summary>
    /// <param name="Id"></param>

    public Habit GetHabit(int Id)
    {
        using var connection = _factory.OpenConnection();
        var command = connection.CreateCommand();

        command.CommandText = $"""
            Select * from Habits
            Where Id = $Id;
        
        """;

        command.Parameters.AddWithValue("$Id", Id);

        using var reader = command.ExecuteReader();

        if (!reader.Read())
        {
            throw new InvalidOperationException($"No habit found with Id {Id}");
        }
        var idOrdinal = reader.GetOrdinal("Id");
        var nameOrdinal = reader.GetOrdinal("Name");
        var quantityOrdinal = reader.GetOrdinal("Quantity");
        var createdAtOrdinal = reader.GetOrdinal("CreatedAt");

        try
        {
            var habit = new Habit()
            {
                HabitId = reader.GetInt32(idOrdinal),
                HabitName = reader.GetString(nameOrdinal),
                HabitQuantity = reader.GetInt32(quantityOrdinal),
                CreatedAt = DateTime.Parse(reader.GetString(createdAtOrdinal))
            };
            return habit;
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Exception converting Habit");
            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine($"Stacktrace: {ex.StackTrace}");
            throw new SqlTypeException();
        }
    }

    public void UpdateHabit(Habit habit)
    {
        using var connection = _factory.OpenConnection();
        var command = connection.CreateCommand();


        command.CommandText = $"""
            Update Habits
            Set 
                Name = $HabitName,
                Quantity = $HabitQuantity,
                CreatedAt = $HabitCreatedAt 
        Where Id = $HabitId;
        """;

        command.Parameters.AddWithValue("$HabitName", habit.HabitName);
        command.Parameters.AddWithValue("$HabitQuantity", habit.HabitQuantity);
        command.Parameters.AddWithValue("$HabitCreatedAt", habit.CreatedAt);
        command.Parameters.AddWithValue("$HabitId", habit.HabitId);
        command.ExecuteNonQuery();
    }

}