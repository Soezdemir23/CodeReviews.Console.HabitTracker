namespace HabitLogger.Models;


/// <summary>
/// This class represents the data in the database for the entry
/// habit:
/// - has a name
/// - has a date
/// - has a quantity 
/// </summary>
public class Habit
{
    public int HabitId { get; set; }
    public string HabitName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int HabitQuantity { get; set; }
}