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
        var test = StringExtensions.ToBigNumber("11111111111111111111111111111111111111111");
        UnityEngine.Debug.Log("ToBigNumberTest" + test);
        UnityEngine.Debug.Log("ToBigNumberTest" + test.ToSimpleString());


        BigNumber bigIntNum1 = StringExtensions.ToBigNumber("123456789012345678901234567890");
        BigNumber bigIntNum2 = StringExtensions.ToBigNumber("987654321098765432109876543210");
        BigNumber myBigNum1 = new BigNumber("123456789012345678901234567890");
        BigNumber myBigNum2 = new BigNumber("987654321098765432109876543210");

        BigNumber bigNum = new BigNumber(0);
        UnityEngine.Debug.Log(bigNum.IsZero);

        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        //for (int i = 0; i < 100000; i++)
        //{
        //    var sum = bigIntNum1 + bigIntNum2;
        //}
        //sw.Stop();
        //UnityEngine.Debug.Log($"BigInteger ���� �ð�: {sw.ElapsedMilliseconds} ms");

        //sw.Restart();
        for (int i = 0; i < 100000; i++)
        {
            var sum = myBigNum1 + myBigNum2;
        }
        sw.Stop();
        UnityEngine.Debug.Log($"BigNum ���� �ð�: {sw.ElapsedMilliseconds} ms");

        //sw.Restart();
        //for (int i = 0; i < 100000; i++)
        //{
        //    var diff = bigIntNum2 - bigIntNum1;
        //}
        //sw.Stop();
        //UnityEngine.Debug.Log($"BigInteger ���� �ð�: {sw.ElapsedMilliseconds} ms");

        sw.Restart();
        for (int i = 0; i < 100000; i++)
        {
            var diff = myBigNum2 - myBigNum1;
        }
        sw.Stop();
        UnityEngine.Debug.Log($"BigNum ���� �ð�: {sw.ElapsedMilliseconds} ms");

        BigInteger bigIntNum3 = BigInteger.Parse("123456789012345678901234567890");
        BigNumber myBigNum3 = StringExtensions.ToBigNumber("123456789012345678901234567890");
        int smallNum = 12345;
        sw.Start();
        //for (int i = 0; i < 100000; i++)
        //{
        //    var sum = bigIntNum3 + smallNum;
        //}
        //sw.Stop();
        //UnityEngine.Debug.Log($"BigInteger + int ���� �ð�: {sw.ElapsedMilliseconds} ms");

        //sw.Restart();
        for (int i = 0; i < 100000; i++)
        {
            var sum = myBigNum3 + smallNum;
        }
        sw.Stop();
        UnityEngine.Debug.Log($"BigNum + int ���� �ð�: {sw.ElapsedMilliseconds} ms");

        //sw.Restart();
        //for (int i = 0; i < 100000; i++)
        //{
        //    var diff = bigIntNum3 - smallNum;
        //}
        //sw.Stop();
        //UnityEngine.Debug.Log($"BigInteger - int ���� �ð�: {sw.ElapsedMilliseconds} ms");

        sw.Restart();
        for (int i = 0; i < 100000; i++)
        {
            var diff = myBigNum3 - smallNum;
        }
        sw.Stop();
        UnityEngine.Debug.Log($"BigNum - int ���� �ð�: {sw.ElapsedMilliseconds} ms");

        //sw.Restart();
        //for (int i = 0; i < 100000; i++)
        //{
        //    var prod = bigIntNum3 * smallNum;
        //}
        //sw.Stop();
        //UnityEngine.Debug.Log($"BigInteger * int ���� �ð�: {sw.ElapsedMilliseconds} ms");

        sw.Restart();
        for (int i = 0; i < 100000; i++)
        {
            var prod = myBigNum3 * smallNum;
        }
        sw.Stop();
        UnityEngine.Debug.Log($"BigNum * int ���� �ð�: {sw.ElapsedMilliseconds} ms");

        //sw.Restart();
        //for (int i = 0; i < 100000; i++)
        //{
        //    var quot = bigIntNum3 / smallNum;
        //}
        //sw.Stop();
        //UnityEngine.Debug.Log($"BigInteger / int ������ �ð�: {sw.ElapsedMilliseconds} ms");

        sw.Restart();
        for (int i = 0; i < 100000; i++)
        {
            var quot = myBigNum3 / smallNum;
        }
        sw.Stop();
        UnityEngine.Debug.Log($"BigNum / int ������ �ð�: {sw.ElapsedMilliseconds} ms");
    }
}
