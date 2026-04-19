namespace HabitLogger.Habit;


/// <summary>
/// This class represents the data in the database for the entry
/// habit:
/// - has a name
/// - has a date
/// - has a quantity 
/// </summary>
public class Habit
{
    public string HabitName { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public int HabitQuantity { get; private set; }
}