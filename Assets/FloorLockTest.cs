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
            CurrencyManager.currency[(int)CurrencyType.Coin] += 50000;
            CurrencyManager.currency[(int)CurrencyType.CopperStone] += 50000;
            CurrencyManager.currency[(int)CurrencyType.SilverStone] += 50000;
            CurrencyManager.currency[(int)CurrencyType.GoldStone] += 50000;
            CurrencyManager.currency[(int)CurrencyType.CopperIngot] += 50000;
            CurrencyManager.currency[(int)CurrencyType.SilverIngot] += 50000;
            CurrencyManager.currency[(int)CurrencyType.GoldIngot] += 50000;
            CurrencyManager.currency[(int)CurrencyType.CopperIngot] += 50000;
        }
    }
}
