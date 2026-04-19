namespace HabitLogger.HabitLogRepository;


/// <summary>
/// This class represents the data in the database for the entry
/// habit:
/// - has a name
/// - has a date
/// - has a quantity 
/// </summary>
public class Habit
{
    public string habitName { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public int habitQuantity { get; private set; }
}