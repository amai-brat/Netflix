namespace PaymentService.Helpers;

public static class Dice
{
    public static bool Flip()
    {
        return Random.Shared.NextDouble() < 0.5;
    }

    public static T Flip<T>(int min = 0, int max = -1) where T : Enum
    {
        var values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(Random.Shared.Next(min, max == -1 
            ? values.Length
            : max))!;
    }
}