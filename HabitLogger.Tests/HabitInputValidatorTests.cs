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
}
