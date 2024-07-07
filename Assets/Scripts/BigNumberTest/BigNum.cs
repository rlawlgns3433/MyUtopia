using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public struct BigNum
{
    static readonly string[] CurrencyUnits = new string[]
       {"", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};
    private List<int> bigNumber;
    private int factor;
    private bool isNegative;

    public BigNum(string number)
    {
        factor = 0;
        bigNumber = new List<int>();
        isNegative = number[0] == '-';

        if (isNegative)
        {
            number = number.Substring(1);
        }
        for (int i = number.Length; i > 0; i -= 3)
        {
            int startIndex = Math.Max(i - 3, 0);
            int length = i - startIndex;
            string chunk = number.Substring(startIndex, length);
            bigNumber.Add(int.Parse(chunk));
        }
        factor = bigNumber.Count - 1;
    }

    public override string ToString()
    {
        if (bigNumber.Count == 0)
            return "0";

        var result = new StringBuilder(isNegative ? "-" : "");
        int index = bigNumber.Count - 1;

        result.Append(bigNumber[index]);

        if (index > 0)
        {
            if (bigNumber[index - 1] >= 100)
            {
                result.Append(".");
                result.Append(bigNumber[index - 1].ToString("D3").Substring(0, 1));
            }
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
        for (int i = 0; i < sb.Length; i++)
        {
            returnUnit.Append(sb[i]);
            returnUnit.Append(sb[i]);
        }
        return returnUnit.ToString();
    }

    public static BigNum operator +(BigNum a, BigNum b)
    {
        if (a.isNegative == b.isNegative)
        {
            var result = Add(a.bigNumber, b.bigNumber);
            return new BigNum { bigNumber = result, isNegative = a.isNegative };
        }
        var comparison = CompareAbsoluteValues(a.bigNumber, b.bigNumber);
        if (comparison == 0)
        {
            return new BigNum("0");
        }
        else if (comparison > 0)
        {
            var result = Subtract(a.bigNumber, b.bigNumber);
            return new BigNum { bigNumber = result, isNegative = a.isNegative };
        }
        else
        {
            var result = Subtract(b.bigNumber, a.bigNumber);
            return new BigNum { bigNumber = result, isNegative = b.isNegative };
        }
    }

    public static BigNum operator -(BigNum a, BigNum b)
    {
        b.isNegative = !b.isNegative;
        return a + b;
    }

    public static BigNum operator *(BigNum a, BigNum b)
    {
        var result = Multiply(a.bigNumber, b.bigNumber);
        return new BigNum { bigNumber = result, isNegative = a.isNegative != b.isNegative };
    }

    public static BigNum operator /(BigNum a, BigNum b)
    {
        var result = Divide(a.bigNumber, b.bigNumber);
        return new BigNum { bigNumber = result, isNegative = a.isNegative != b.isNegative };
    }

    private static List<int> Add(List<int> a, List<int> b)
    {
        var result = new List<int>();
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

    private static List<int> Subtract(List<int> a, List<int> b)
    {
        var result = new List<int>();
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
        return new BigNum { bigNumber = result, isNegative = this.isNegative };
    }

    public BigNum Minus(BigNum a)
    {
        a.isNegative = !a.isNegative;
        return this + a;
    }

    public BigNum Multiply(BigNum a)
    {
        var result = Multiply(bigNumber, a.bigNumber);
        return new BigNum { bigNumber = result, isNegative = this.isNegative != a.isNegative };
    }

    public BigNum Divide(BigNum a)
    {
        var result = Divide(bigNumber, a.bigNumber);
        return new BigNum { bigNumber = result, isNegative = this.isNegative != a.isNegative };
    }
}
