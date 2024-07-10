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
            CurrencyManager.currency[(int)CurrencyType.CopperIngot] += 50000;
        }
    }
}
