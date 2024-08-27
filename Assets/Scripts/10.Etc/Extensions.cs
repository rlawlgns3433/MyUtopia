using System.Collections.Generic;
using System;
using System.Numerics;
using Unity.VisualScripting;
using System.Text;

public static class BigIntegerExtensions
{
    static readonly string[] CurrencyUnits = new string[]
{
        "", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
};

    private static readonly char[] Suffixes = { ' ', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
    private static readonly BigInteger thousand = thousand = new BigInteger(1000);

    public static string ToCustomString(this BigInteger value)
    {
        if (value < 1000)
        {
            return value.ToString();
        }

        int suffixIndex = 0;
        BigInteger rem = new BigInteger();

        while (value >= 1000)
        {
            value = BigInteger.DivRem(value, 1000, out rem);
            suffixIndex++;
        }

        var result = new StringBuilder();
        result.Append($"{value}.{rem:D3}");

        if (suffixIndex < CurrencyUnits.Length)
        {
            result.Append(CurrencyUnits[suffixIndex]);
        }
        else
        {
            result.Append(GetCurrencyUnit(suffixIndex));
        }

        return result.ToString();
    }

    private static string GetCurrencyUnit(int index)
    {
        // 예시: 7번째 이상 단위를 위한 로직 (Z, Y 등)
        // 예: Z, Y, 또는 추가적인 단위들을 생성하는 로직을 추가할 수 있습니다.
        char suffix = (char)('a' + (index - CurrencyUnits.Length));
        return suffix.ToString().ToUpper();
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
    public static BigNumber ToBigNumber(this string value)
    {
        return new BigNumber(value);
    }
}



