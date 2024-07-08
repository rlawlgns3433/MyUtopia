using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigNumberTest : MonoBehaviour
{
    void Start()
    {
        BigNum num1 = new BigNum("12321321321421321311231231323132332313123213213123123131241242112324235346346456435532234324321123");
        BigNum num2 = new BigNum("13213214325325355343432312312321332312332314327463376123213231232131321313213121231231231231221321");
        BigNum num3 = new BigNum("5323232312");
        BigNum num4 = new BigNum("4323232312");
        Debug.Log($"test : {num1.ToString()}");
        var sum = num1 + num2;


        Debug.Log($"{num2} - {num1} = {num2 - num1}");

        Debug.Log($"{num1} - {num3} = {num1 - num3}");

        Debug.Log($"{num1} * {num3} = {num1 * num3}");
        Debug.Log($"{num1} + {num2} = {sum}");
        Debug.Log($"{num1.Plus(num2)}");
        Debug.Log($"{num1} - {num2} = {num1 - num2}");
        Debug.Log($"{num1.Minus(num2)}");
        Debug.Log($"{num1} * {num2} = {num1 * num2}");
        Debug.Log($"{num1.Multiply(num2)}");
        Debug.Log($"{num1} / {num2} = {num1 / num2}");
        Debug.Log($"{num1.Divide(num2)}");


        //Debug.Log($"{num3} - {num4} = {num3 - num4}");
        //Debug.Log($"{num3.Minus(num4)}");
    }
}
