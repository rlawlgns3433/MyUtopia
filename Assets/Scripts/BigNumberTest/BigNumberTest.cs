using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using UnityEngine;

public class BigNumberTest : MonoBehaviour
{
    void Start()
    {
        BigInteger bigIntNum1 = BigInteger.Parse("123456789012345678901234567890");
        BigInteger bigIntNum2 = BigInteger.Parse("987654321098765432109876543210");
        BigNum myBigNum1 = new BigNum("123456789012345678901234567890");
        BigNum myBigNum2 = new BigNum("987654321098765432109876543210");

        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        for (int i = 0; i < 100000; i++)
        {
            var sum = bigIntNum1 + bigIntNum2;
        }
        sw.Stop();
        UnityEngine.Debug.Log($"BigInteger µ¡¼À ½Ã°£: {sw.ElapsedMilliseconds} ms");

        sw.Restart();
        for (int i = 0; i < 100000; i++)
        {
            var sum = myBigNum1 + myBigNum2;
        }
        sw.Stop();
        UnityEngine.Debug.Log($"BigNum µ¡¼À ½Ã°£: {sw.ElapsedMilliseconds} ms");

        sw.Restart();
        for (int i = 0; i < 100000; i++)
        {
            var diff = bigIntNum2 - bigIntNum1;
        }
        sw.Stop();
        UnityEngine.Debug.Log($"BigInteger »¬¼À ½Ã°£: {sw.ElapsedMilliseconds} ms");

        sw.Restart();
        for (int i = 0; i < 100000; i++)
        {
            var diff = myBigNum2 - myBigNum1;
        }
        sw.Stop();
        UnityEngine.Debug.Log($"BigNum »¬¼À ½Ã°£: {sw.ElapsedMilliseconds} ms");

        BigInteger bigIntNum3 = BigInteger.Parse("123456789012345678901234567890");
        BigNum myBigNum3 = new BigNum("123456789012345678901234567890");
        int smallNum = 12345;
        sw.Start();
        for (int i = 0; i < 100000; i++)
        {
            var sum = bigIntNum3 + smallNum;
        }
        sw.Stop();
        UnityEngine.Debug.Log($"BigInteger + int µ¡¼À ½Ã°£: {sw.ElapsedMilliseconds} ms");

        sw.Restart();
        for (int i = 0; i < 100000; i++)
        {
            var sum = myBigNum3 + smallNum;
        }
        sw.Stop();
        UnityEngine.Debug.Log($"BigNum + int µ¡¼À ½Ã°£: {sw.ElapsedMilliseconds} ms");

        sw.Restart();
        for (int i = 0; i < 100000; i++)
        {
            var diff = bigIntNum3 - smallNum;
        }
        sw.Stop();
        UnityEngine.Debug.Log($"BigInteger - int »¬¼À ½Ã°£: {sw.ElapsedMilliseconds} ms");

        sw.Restart();
        for (int i = 0; i < 100000; i++)
        {
            var diff = myBigNum3 - smallNum;
        }
        sw.Stop();
        UnityEngine.Debug.Log($"BigNum - int »¬¼À ½Ã°£: {sw.ElapsedMilliseconds} ms");

        sw.Restart();
        for (int i = 0; i < 100000; i++)
        {
            var prod = bigIntNum3 * smallNum;
        }
        sw.Stop();
        UnityEngine.Debug.Log($"BigInteger * int °ö¼À ½Ã°£: {sw.ElapsedMilliseconds} ms");

        sw.Restart();
        for (int i = 0; i < 100000; i++)
        {
            var prod = myBigNum3 * smallNum;
        }
        sw.Stop();
        UnityEngine.Debug.Log($"BigNum * int °ö¼À ½Ã°£: {sw.ElapsedMilliseconds} ms");

        sw.Restart();
        for (int i = 0; i < 100000; i++)
        {
            var quot = bigIntNum3 / smallNum;
        }
        sw.Stop();
        UnityEngine.Debug.Log($"BigInteger / int ³ª´°¼À ½Ã°£: {sw.ElapsedMilliseconds} ms");

        sw.Restart();
        for (int i = 0; i < 100000; i++)
        {
            var quot = myBigNum3 / smallNum;
        }
        sw.Stop();
        UnityEngine.Debug.Log($"BigNum / int ³ª´°¼À ½Ã°£: {sw.ElapsedMilliseconds} ms");
    }
}
