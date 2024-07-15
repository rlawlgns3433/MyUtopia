using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public struct BigNumber
{
    static readonly string[] CurrencyUnits = new string[]
    {
        "", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
    };

    private List<int> bigNumber;

    public static readonly BigNumber Zero = new BigNumber(0);

    private bool isZero;
    public bool IsZero
    {
        get
        {
            if (bigNumber == null || bigNumber.Count == 0)
                return true;

            foreach (var element in bigNumber)
            {
                if (element != 0)
                    return false;
            }
            return true;
        }
        private set { isZero = value; }
    }

    public BigNumber(string number)
    {
        bigNumber = new List<int>((number.Length + 2) / 3);
        isZero = false;

        if (string.IsNullOrEmpty(number) || number == "0")
        {
            this = Zero;
            return;
        }

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

        isZero = this.IsZero;
    }

    public BigNumber(int number)
    {
        bigNumber = new List<int>();
        isZero = number == 0;

        if (number == 0)
        {
            bigNumber.Add(0);
        }
        else
        {
            do
            {
                bigNumber.Add(number % 1000);
                number /= 1000;
            } while (number > 0);
        }
    }

    public override string ToString()
    {
        if (bigNumber == null || bigNumber.Count == 0)
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
        if (bigNumber == null || bigNumber.Count == 0)
            return "0";

        var result = new StringBuilder();
        for (int i = bigNumber.Count - 1; i >= 0; i--)
        {
            result.Append(i == bigNumber.Count - 1 ? bigNumber[i].ToString() : bigNumber[i].ToString("D3"));
        }
        return result.ToString().TrimStart('0');
    }
    public BigNumber ParseBigNumber(string number)
    {
        var tempBigNumber = new List<int>((number.Length + 2) / 3);
        isZero = false;

        if (string.IsNullOrEmpty(number) || number == "0")
        {
            return Zero;
        }

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

        return new BigNumber { bigNumber = tempBigNumber };
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

    private static List<int> ConvertIntToBigNumber(int b)
    {
        var bBigNumber = new List<int>();
        do
        {
            bBigNumber.Add(b % 1000);
            b /= 1000;
        } while (b > 0);
        return bBigNumber;
    }

    public static bool operator >(BigNumber a, int b)
    {
        if (a.bigNumber == null)
        {
            return b < 0;
        }

        var aBigNumber = a.bigNumber;
        var bBigNumber = ConvertIntToBigNumber(b);
        if (aBigNumber.Count > bBigNumber.Count)
        {
            return true;
        }
        else if (aBigNumber.Count == bBigNumber.Count)
        {
            for (int i = aBigNumber.Count - 1; i >= 0; --i)
            {
                if (aBigNumber[i] == bBigNumber[i])
                    continue;
                else if (aBigNumber[i] > bBigNumber[i])
                    return true;
                else
                    return false;
            }
        }
        return false;
    }

    public static bool operator <(BigNumber a, int b)
    {
        return !(a >= b);
    }

    public static bool operator >=(BigNumber a, int b)
    {
        if (a.bigNumber == null)
        {
            return b <= 0;
        }

        var aBigNumber = a.bigNumber;
        var bBigNumber = ConvertIntToBigNumber(b);
        if (aBigNumber.Count > bBigNumber.Count)
        {
            return true;
        }
        else if (aBigNumber.Count == bBigNumber.Count)
        {
            for (int i = aBigNumber.Count - 1; i >= 0; --i)
            {
                if (aBigNumber[i] == bBigNumber[i])
                    continue;
                else if (aBigNumber[i] > bBigNumber[i])
                    return true;
                else
                    return false;
            }
            return true;
        }
        return false;
    }

    public static bool operator <=(BigNumber a, int b)
    {
        return !(a > b);
    }

    public static bool operator ==(BigNumber a, int b)
    {
        if (a.bigNumber == null)
        {
            return b == 0;
        }

        var aBigNumber = a.bigNumber;
        var bBigNumber = ConvertIntToBigNumber(b);
        if (aBigNumber.Count == bBigNumber.Count)
        {
            for (int i = 0; i < aBigNumber.Count; ++i)
            {
                if (aBigNumber[i] == bBigNumber[i])
                    continue;
                else
                    return false;
            }
            return true;
        }
        else
            return false;
    }

    public static bool operator !=(BigNumber a, int b)
    {
        return !(a == b);
    }

    public static bool operator >(BigNumber a, BigNumber b)
    {
        if (a.bigNumber == null)
        {
            return b > 0;
        }

        var aBigNumber = a.bigNumber;
        var bBigNumber = b.bigNumber;
        if (aBigNumber.Count > bBigNumber.Count)
        {
            return true;
        }
        else if (aBigNumber.Count == bBigNumber.Count)
        {
            for (int i = aBigNumber.Count - 1; i >= 0; --i)
            {
                if (aBigNumber[i] == bBigNumber[i])
                    continue;
                else if (aBigNumber[i] > bBigNumber[i])
                    return true;
                else
                    return false;
            }
        }
        return false;
    }

    public static bool operator <(BigNumber a, BigNumber b)
    {
        if (a.bigNumber == null)
        {
            return b < 0;
        }

        var aBigNumber = a.bigNumber;
        var bBigNumber = b.bigNumber;
        if (aBigNumber.Count < bBigNumber.Count)
        {
            return true;
        }
        else if (aBigNumber.Count == bBigNumber.Count)
        {
            for (int i = aBigNumber.Count - 1; i >= 0; --i)
            {
                if (aBigNumber[i] == bBigNumber[i])
                    continue;
                else if (aBigNumber[i] < bBigNumber[i])
                    return true;
                else
                    return false;
            }
        }
        return false;
    }

    public static BigNumber operator +(BigNumber a, BigNumber b)
    {
        var result = Add(a.bigNumber, b.bigNumber);
        return new BigNumber { bigNumber = result };
    }

    public static BigNumber operator -(BigNumber a, BigNumber b)
    {
        var result = Subtract(a.bigNumber, b.bigNumber);
        return new BigNumber { bigNumber = result };
    }

    public static BigNumber operator *(BigNumber a, BigNumber b)
    {
        var result = Multiply(a.bigNumber, b.bigNumber);
        return new BigNumber { bigNumber = result };
    }

    public static BigNumber operator /(BigNumber a, BigNumber b)
    {
        var result = Divide(a.bigNumber, b.bigNumber);
        return new BigNumber { bigNumber = result };
    }

    public static BigNumber operator +(BigNumber a, int b)
    {
        return Add(a, b);
    }

    public static BigNumber operator -(BigNumber a, int b)
    {
        return Subtract(a, b);
    }

    public static BigNumber operator *(BigNumber a, int b)
    {
        return Multiply(a, b);
    }

    public static BigNumber operator /(BigNumber a, int b)
    {
        return Divide(a, b);
    }

    private static BigNumber Add(BigNumber a, int b)
    {
        if (b == 0)
            return a;
        if (a.bigNumber == null)
        {
            a.bigNumber = new List<int>();
        }

        var result = new BigNumber();
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

    private static BigNumber Subtract(BigNumber a, int b)
    {
        if (b == 0)
            return a;

        var result = new BigNumber();
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
        if (a == null)
        {
            a = new List<int>();
        }
        if (b == null)
        {
            b = new List<int>();
        }
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

    private static BigNumber Multiply(BigNumber a, int b)
    {
        if (a.bigNumber == null)
        {
            return Zero;
        }
        if (b == 0)
            return new BigNumber(0);
        if (b == 1)
            return a;

        var result = new BigNumber();
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

    private static BigNumber Divide(BigNumber a, int b)
    {
        if(a.bigNumber == null)
        { return Zero; }
        if (b == 0)
            return new BigNumber(0);
        if (b == 1)
            return a;

        var result = new BigNumber();
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
        if (a == null)
        {
            a = new List<int>();
        }
        if (b == null)
        {
            b = new List<int>();
        }
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
        if (a == null)
        {
            a = new List<int>();
        }
        if (b == null)
        {
            b = new List<int>();
        }
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
        if (a == null)
        {
            a = new List<int>();
        }
        if (b == null)
        {
            b = new List<int>();
        }
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
        if (a == null && b == null) return 0;
        if (a == null) return -1;
        if (b == null) return 1;

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
    public BigNumber Plus(BigNumber a)
    {
        var result = Add(bigNumber, a.bigNumber);
        return new BigNumber { bigNumber = result };
    }

    public BigNumber Minus(BigNumber a)
    {
        return this - a;
    }

    public BigNumber Multiply(BigNumber a)
    {
        var result = Multiply(bigNumber, a.bigNumber);
        return new BigNumber { bigNumber = result };
    }

    public BigNumber Divide(BigNumber a)
    {
        var result = Divide(bigNumber, a.bigNumber);
        return new BigNumber { bigNumber = result };
    }

    public BigNumber Clear()
    {
        var result = bigNumber;
        for (int i = 0; i < result.Count; ++i)
        {
            result[i] = 0;
        }
        return new BigNumber { bigNumber = result };
    }
    public float ToFloat()
    {
        if (bigNumber == null || bigNumber.Count == 0)
            return 0f;

        float result = 0f;
        for (int i = bigNumber.Count - 1; i >= 0; i--)
        {
            result = result * 1000 + bigNumber[i];
        }

        return result;
    }

    public static float ToFloatClamped01(BigNumber value, BigNumber max)
    {
        float result = value.ToFloat();
        float maxValue = max.ToFloat();
        return Mathf.Clamp(result / maxValue, 0f, 1f);
    }
}
