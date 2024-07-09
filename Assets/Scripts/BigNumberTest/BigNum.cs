using System;
using System.Collections.Generic;
using System.Text;

public struct BigNum
{
    static readonly string[] CurrencyUnits = new string[]
    {
        "", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
    };

    private List<int> bigNumber;

    public BigNum(string number)
    {
        bigNumber = new List<int>((number.Length + 2) / 3);
        if (number.Contains("."))
        {
            var parts = number.Split('.');
            number = parts[0] + parts[1];
        }

        for (int i = number.Length; i > 0; i -= 3)
        {
            int startIndex = Math.Max(i - 3, 0);
            int length = i - startIndex;
            string chunk = number.Substring(startIndex, length);
            bigNumber.Add(int.Parse(chunk));
        }
    }

    public BigNum(int number)
    {
        bigNumber = new List<int>();

        do
        {
            bigNumber.Add(number % 1000);
            number /= 1000;
        } while (number > 0);
    }

    public override string ToString()
    {
        if (bigNumber.Count == 0)
            return "0";

        var result = new StringBuilder();
        int index = bigNumber.Count - 1;

        result.Append(bigNumber[index]);

        if (index > 0 && bigNumber[index - 1] >= 100)
        {
            result.Append(".");
            result.Append(bigNumber[index - 1].ToString("D3").Substring(0, 1));
        }

        if (index < CurrencyUnits.Length)
        {
            result.Append(CurrencyUnits[index]);
        }
        else
        {
            result.Append(GetCurrencyUnit(index));
        }

        return result.ToString();
    }

    public string ToSimpleString()
    {
        if (bigNumber.Count == 0)
            return "0";

        var result = new StringBuilder();
        for (int i = bigNumber.Count - 1; i >= 0; i--)
        {
            result.Append(i == bigNumber.Count - 1 ? bigNumber[i].ToString() : bigNumber[i].ToString("D3"));
        }
        return result.ToString().TrimStart('0');
    }

    private static string GetCurrencyUnit(int index)
    {
        index -= CurrencyUnits.Length;
        var sb = new StringBuilder();
        while (index >= 0)
        {
            int charIndex = index % CurrencyUnits.Length;
            sb.Insert(0, CurrencyUnits[charIndex]);
            index = (index / CurrencyUnits.Length) - 1;
        }
        var returnUnit = new StringBuilder();
        foreach (var ch in sb.ToString())
        {
            returnUnit.Append(ch).Append(ch);
        }
        return returnUnit.ToString();
    }

    public static BigNum operator +(BigNum a, BigNum b)
    {
        var result = Add(a.bigNumber, b.bigNumber);
        return new BigNum { bigNumber = result };
    }

    public static BigNum operator -(BigNum a, BigNum b)
    {
        var result = Subtract(a.bigNumber, b.bigNumber);
        return new BigNum { bigNumber = result };
    }

    public static BigNum operator *(BigNum a, BigNum b)
    {
        var result = Multiply(a.bigNumber, b.bigNumber);
        return new BigNum { bigNumber = result };
    }

    public static BigNum operator /(BigNum a, BigNum b)
    {
        var result = Divide(a.bigNumber, b.bigNumber);
        return new BigNum { bigNumber = result };
    }

    public static BigNum operator +(BigNum a, int b)
    {
        return Add(a, b);
    }

    public static BigNum operator -(BigNum a, int b)
    {
        return Subtract(a, b);
    }

    public static BigNum operator *(BigNum a, int b)
    {
        return Multiply(a, b);
    }

    public static BigNum operator /(BigNum a, int b)
    {
        return Divide(a, b);
    }

    private static BigNum Add(BigNum a, int b)
    {
        if (b == 0)
            return a;

        var result = new BigNum();
        result.bigNumber = new List<int>(a.bigNumber);
        int value = b;
        for (int i = 0; i < result.bigNumber.Count || value > 0; i++)
        {
            if (i == result.bigNumber.Count)
            {
                result.bigNumber.Add(0);
            }
            int sum = result.bigNumber[i] + value;
            result.bigNumber[i] = sum % 1000;
            value = sum / 1000;
        }
        return result;
    }

    private static BigNum Subtract(BigNum a, int b)
    {
        if (b == 0)
            return a;

        var result = new BigNum();
        result.bigNumber = new List<int>(a.bigNumber);
        var bList = new List<int>();
        while (b > 0)
        {
            bList.Add(b % 1000);
            b /= 1000;
        }

        int borrow = 0;
        for (int i = 0; i < result.bigNumber.Count || i < bList.Count; i++)
        {
            int aValue = i < result.bigNumber.Count ? result.bigNumber[i] : 0;
            int bValue = i < bList.Count ? bList[i] : 0;

            int diff = aValue - bValue - borrow;
            if (diff < 0)
            {
                diff += 1000;
                borrow = 1;
            }
            else
            {
                borrow = 0;
            }

            if (i < result.bigNumber.Count)
            {
                result.bigNumber[i] = diff;
            }
            else
            {
                result.bigNumber.Add(diff);
            }
        }
        for (int i = result.bigNumber.Count - 1; i > 0; i--)
        {
            if (result.bigNumber[i] == 0)
            {
                result.bigNumber.RemoveAt(i);
            }
            else
            {
                break;
            }
        }
        return result;
    }

    private static List<int> Subtract(List<int> a, List<int> b)
    {
        var result = new List<int>(a.Count);
        int borrow = 0;

        for (int i = 0; i < a.Count; i++)
        {
            int diff = a[i] - (i < b.Count ? b[i] : 0) - borrow;
            if (diff < 0)
            {
                diff += 1000;
                borrow = 1;
            }
            else
            {
                borrow = 0;
            }

            result.Add(diff);
        }

        for (int i = result.Count - 1; i > 0; i--)
        {
            if (result[i] == 0)
            {
                result.RemoveAt(i);
            }
            else
            {
                break;
            }
        }

        return result;
    }

    private static BigNum Multiply(BigNum a, int b)
    {
        if (b == 0)
            return new BigNum(0);
        if (b == 1)
            return a;

        var result = new BigNum();
        result.bigNumber = new List<int>(a.bigNumber.Count + 1);

        int carry = 0;
        for (int i = 0; i < a.bigNumber.Count; i++)
        {
            long mul = (long)a.bigNumber[i] * b + carry;
            result.bigNumber.Add((int)(mul % 1000));
            carry = (int)(mul / 1000);
        }
        if (carry > 0)
        {
            result.bigNumber.Add(carry);
        }
        return result;
    }

    private static BigNum Divide(BigNum a, int b)
    {
        if (b == 0)
            return new BigNum(0);
        if (b == 1)
            return a;

        var result = new BigNum();
        result.bigNumber = new List<int>(a.bigNumber.Count);
        int carry = 0;
        for (int i = a.bigNumber.Count - 1; i >= 0; i--)
        {
            long cur = a.bigNumber[i] + carry * 1000L;
            result.bigNumber.Insert(0, (int)(cur / b));
            carry = (int)(cur % b);
        }

        while (result.bigNumber.Count > 1 && result.bigNumber[result.bigNumber.Count - 1] == 0)
        {
            result.bigNumber.RemoveAt(result.bigNumber.Count - 1);
        }

        return result;
    }

    private static List<int> Add(List<int> a, List<int> b)
    {
        var result = new List<int>(Math.Max(a.Count, b.Count) + 1);
        int carry = 0;
        int length = Math.Max(a.Count, b.Count);

        for (int i = 0; i < length || carry != 0; i++)
        {
            int sum = carry;
            if (i < a.Count) sum += a[i];
            if (i < b.Count) sum += b[i];

            carry = sum / 1000;
            result.Add(sum % 1000);
        }

        return result;
    }

    private static List<int> Multiply(List<int> a, List<int> b)
    {
        var result = new List<int>(new int[a.Count + b.Count]);

        for (int i = 0; i < a.Count; i++)
        {
            for (int j = 0; j < b.Count; j++)
            {
                long mul = (long)a[i] * b[j];
                result[i + j] += (int)(mul % 1000);
                result[i + j + 1] += (int)(mul / 1000);

                if (result[i + j] >= 1000)
                {
                    result[i + j + 1] += result[i + j] / 1000;
                    result[i + j] %= 1000;
                }
            }
        }

        while (result.Count > 1 && result[result.Count - 1] == 0)
        {
            result.RemoveAt(result.Count - 1);
        }

        return result;
    }

    private static List<int> Divide(List<int> a, List<int> b)
    {
        var result = new List<int>();
        var dividend = new List<int>(a);
        var divisor = new List<int>(b);
        var temp = new List<int>();

        for (int i = dividend.Count - 1; i >= 0; i--)
        {
            temp.Insert(0, dividend[i]);
            int count = 0;

            while (CompareAbsoluteValues(temp, divisor) >= 0)
            {
                temp = Subtract(temp, divisor);
                count++;
            }

            result.Insert(0, count);
        }

        while (result.Count > 1 && result[result.Count - 1] == 0)
        {
            result.RemoveAt(result.Count - 1);
        }

        return result;
    }

    private static int CompareAbsoluteValues(List<int> a, List<int> b)
    {
        if (a.Count != b.Count)
        {
            return a.Count.CompareTo(b.Count);
        }

        for (int i = a.Count - 1; i >= 0; i--)
        {
            if (a[i] != b[i])
            {
                return a[i].CompareTo(b[i]);
            }
        }
        return 0;
    }

    public BigNum Plus(BigNum a)
    {
        var result = Add(bigNumber, a.bigNumber);
        return new BigNum { bigNumber = result };
    }

    public BigNum Minus(BigNum a)
    {
        return this - a;
    }

    public BigNum Multiply(BigNum a)
    {
        var result = Multiply(bigNumber, a.bigNumber);
        return new BigNum { bigNumber = result };
    }

    public BigNum Divide(BigNum a)
    {
        var result = Divide(bigNumber, a.bigNumber);
        return new BigNum { bigNumber = result };
    }
}
