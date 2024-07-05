using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigNumberTest : MonoBehaviour
{
    void Start()
    {
        BigNum num1 = new BigNum("12321321321421123");
        BigNum num2 = new BigNum("23213214325325355343432432746337612321321");
        BigNum num3 = new BigNum("5323232312");
        Debug.Log($"test : {num1}");
        var sum = num1 + num2;
        Debug.Log($"{num1} + {num2} = {sum}");

        Debug.Log($"{num2} - {num1} = {num2 - num1}");
        Debug.Log($"{num1} - {num2} = {num1 - num2}");
        Debug.Log($"{num1} - {num3} = {num1 - num3}");

        Debug.Log($"{num1} * {num2} = {num1 * num2}");
        Debug.Log($"{num1} * {num3} = {num1 * num3}");
        Debug.Log($"{num1.Plus(num2)}");
    }
}
