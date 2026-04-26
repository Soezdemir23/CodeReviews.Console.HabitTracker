using HabitLogger.DatabaseBootstrapper;

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


    public void Addhabit(string habit)
    {
        var connection = _factory.OpenConnection();
        var command = connection.CreateCommand();

        command.CommandText = $"""
        {habit}
        
        """;
    }

}