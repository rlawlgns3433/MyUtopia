using System.Numerics;

public static class BigIntegerExtensions
{
    private static readonly char[] Suffixes = { ' ', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
    private static readonly BigInteger thousand = thousand = new BigInteger(1000);

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

public static class StringExtensions
{
    private static readonly BigInteger thousand = new BigInteger(1000);
    private static BigInteger significant = default;
    private static BigInteger floating = default;
    public static int suffixAscii = 97;
    public static BigInteger ToBigInteger(this string value)
    {
        significant = default;

        char suffix = value[value.Length - 1];

        if(suffix < 97)
        {
            return significant = BigInteger.Parse(value);
        }

        int dotIndex = value.IndexOf('.');
        significant = BigInteger.Parse(value.Substring(0, dotIndex));
        floating = BigInteger.Parse(value.Substring(dotIndex + 1, 3));

        int exp = suffix - suffixAscii;

        return significant * BigInteger.Pow(thousand, exp);
    }
}
