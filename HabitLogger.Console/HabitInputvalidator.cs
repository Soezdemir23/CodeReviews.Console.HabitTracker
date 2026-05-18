
public class HabitInputValidator
{
    public static bool IsValidQuantity(int quantity)
    {
        return quantity > 0 && quantity < 9999;
    }

    public static bool TryParseHabitDate(string input, out DateTime date)
    {
        return DateTime.TryParseExact(
            input,
            "yyyy-MM-dd",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None,
            out date
        );
    }

    public static string FormatDateForStorage(DateTime date)
    {
        return date.ToString("yyyy-MM-dd HH:mm:ss");
    }
}