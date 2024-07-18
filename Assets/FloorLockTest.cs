using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorLockTest : MonoBehaviour
{
    public Floor floor;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            floor.LevelUp();
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            CurrencyManager.currency[CurrencyType.Coin] += 50000;
            CurrencyManager.currency[CurrencyType.CopperStone] += 50000;
            CurrencyManager.currency[CurrencyType.SilverStone] += 50000;
            CurrencyManager.currency[CurrencyType.GoldStone] += 50000;
            CurrencyManager.currency[CurrencyType.CopperIngot] += 50000;
            CurrencyManager.currency[CurrencyType.SilverIngot] += 50000;
            CurrencyManager.currency[CurrencyType.GoldIngot] += 50000;
        }
    }

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
