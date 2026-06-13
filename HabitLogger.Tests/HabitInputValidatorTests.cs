namespace HabitLogger.Tests;

public class HabitInputValidatorTests
{
    [Test]
    public void IsValidQuantity_ReturnsTrue_WhenQuantityIsInRange()
    {
        var result = HabitInputValidator.IsValidQuantity(5);

        Assert.That(result, Is.True);
    }

    [Test]
    public void IsValidQuantity_ReturnsFalse_WhenQuantityIsZero()
    {
        var result = HabitInputValidator.IsValidQuantity(0);

        Assert.That(result, Is.False);
    }

    [Test]
    public void IsValidQuantity_ReturnsFalse_WhenQuantityIsOver10KMinus1()
    {
        var result = HabitInputValidator.IsValidQuantity(9999);

        Assert.That(result, Is.False);
    }



    [Test]
    public void TryParseHabitDate_ReturnsTrue_ForValidNormalDate()
    {
        var result = HabitInputValidator.TryParseHabitDate("2026-05-17", out var date);

        Assert.That(result, Is.True);
        Assert.That(date, Is.EqualTo(new DateTime(2026, 5, 17)));
    }

    [Test]
    public void TryParseHabitDate_ReturnsFalse_ForInvalidDate()
    {

        var result = HabitInputValidator.TryParseHabitDate("17/05/2026", out var date);

        Assert.That(result, Is.False);
        // default for Datetime is 0001-01-01 00:00:00
        Assert.That(date, Is.EqualTo(default(DateTime)));
    }

    [TestCase("2024-02-29", true)]
    [TestCase("2025-02-29", false)]
    [TestCase("2020-02-29", true)]
    [TestCase("2016-02-29", true)]
    [TestCase("2028-02-29", true)]
    [TestCase("2000-02-29", true)]
    [TestCase("2023-02-29", false)]
    [TestCase("2019-02-29", false)]
    [TestCase("1900-02-29", false)]
    [TestCase("2100-02-29", false)]
    public void TryParseHabitDate_ValidateForLeapYear(string input, bool expected)
    {
        var result = HabitInputValidator.TryParseHabitDate(input, out var date);

        Assert.That(result, Is.EqualTo(expected));

        if (!expected)
        {
            Assert.That(date, Is.EqualTo(default(DateTime)));
        }
    }



    [TestCase("2026-04-30", true)]
    [TestCase("2026-04-31", false)]
    [TestCase("2026-01-31", true)]
    [TestCase("2026-01-32", false)]
    [TestCase("2026-02-28", true)]
    [TestCase("2026-02-29", false)]
    [TestCase("2028-02-28", true)]
    [TestCase("2000-02-28", true)]
    [TestCase("2023-02-28", true)]
    [TestCase("2019-02-28", true)]
    public void TryParseHabitDate_ValidateForMonthsEnd(string input, bool expected)
    {
        var result = HabitInputValidator.TryParseHabitDate(input, out var date);

        Assert.That(result, Is.EqualTo(expected));

        if (!expected)
        {
            Assert.That(date, Is.EqualTo(default(DateTime)));
        }
    }


    [TestCase("2026/05/17", false)]
    [TestCase("17-05-2026", false)]
    [TestCase("", false)]
    public void TryParseHabitDate_RejectsInvalidFormats(string input, bool expected)
    {
        var result = HabitInputValidator.TryParseHabitDate(input, out var date);
        Assert.That(result, Is.EqualTo(expected));

        //if the test fails (expected), check if the  out date value is the default value
        //Datetime.MinValue was recommended by AI, this method will get it applied to
        //express the synonymity of each method in that code block
        if (!expected)
        {
            Assert.That(date, Is.EqualTo(DateTime.MinValue));
        }
    }

}
