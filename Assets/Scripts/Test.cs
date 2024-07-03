using System.Numerics;
using UnityEngine;

public class Test : MonoBehaviour
{
    private static readonly int bigIntegerCount = 2;
    private float timer = 0f;
    public float interval = 0.05f;
    BigInteger[] big = new BigInteger[bigIntegerCount];

    void Start()
    {
        for(int i = 0; i < bigIntegerCount; ++i)
        {
            big[i] = new BigInteger();
            //big[i] = BigInteger.Pow(BigInteger.Parse("999"), i);
            //Debug.Log(big[i].FormatBigInteger());
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > interval)
        {
            for (int i = 0; i < bigIntegerCount; ++i)
            {
                big[i] += BigInteger.Pow(BigInteger.Parse("999"), i);
                Debug.Log(big[i].FormatBigInteger());
            }
            timer = 0f;
        }
    }
}