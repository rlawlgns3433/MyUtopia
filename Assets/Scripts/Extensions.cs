using System.Numerics;

public static class BigIntegerExtensions
{
    private static readonly string[] Suffixes = { "", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
    private static readonly BigInteger thousand = new BigInteger(1000);

    public static string FormatBigInteger(this BigInteger value)
    {
        if (value < 1000)
        {
            return value.ToString();
        }

        int suffixIndex = 0;
        BigInteger rem = new BigInteger();
        while (value >= 1000)
        {
            value = BigInteger.DivRem(value, thousand, out rem);

            suffixIndex++;
        }

        return $"{value}.{rem:D3}{Suffixes[suffixIndex]}";
    }
}