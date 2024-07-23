using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorLockTest : MonoBehaviour
{
    public Floor floor;

    public void ShowMeTheMoney()
    {
        CurrencyManager.currency[CurrencyType.Coin] += 10000;
        CurrencyManager.currency[CurrencyType.CopperStone] += 10000;
        CurrencyManager.currency[CurrencyType.SilverStone] += 10000;
        CurrencyManager.currency[CurrencyType.GoldStone] += 10000;
        CurrencyManager.currency[CurrencyType.CopperIngot] += 10000;
        CurrencyManager.currency[CurrencyType.SilverIngot] += 10000;
        CurrencyManager.currency[CurrencyType.GoldIngot] += 10000;
    }
}
