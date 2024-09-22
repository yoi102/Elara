namespace Commons.Extensions;

public static class RandomExtensions
{
    public static double NextDouble(this Random random, double minValue, double maxValue)
    {
        if (minValue >= maxValue)
        {
            throw new ArgumentOutOfRangeException("minValue", "minValue cannot be bigger than maxValue");
        }

        double num = random.NextDouble();
        return num * maxValue + (1.0 - num) * minValue;
    }
}