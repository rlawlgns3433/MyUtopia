using Cysharp.Threading.Tasks;
using System;
using System.Linq;

[Serializable]
public class StorageData
{
    private string currWorkLoad;
    public BigNumber CurrentWorkLoad
    {
        get
        {
            return new BigNumber(currWorkLoad);
        }
        set
        {
            currWorkLoad = value.ToSimpleString();
        }
    }
    private string[] currArray;
    public BigNumber[] CurrArray
    {
        get
        {
            return currArray.Select(s => new BigNumber(s)).ToArray();
        }
        set
        {
            currArray = value.Select(bn => bn.ToSimpleString()).ToArray();
        }
    }
    private int totalOfflineTime;
    public int TotalOfflineTime
    {
        get
        {
            return totalOfflineTime;
        }
        set
        {
            totalOfflineTime = value;
        }
    }
}

public class Storage : Building
{
    private void Awake()
    {
        clickEvent += OpenProductStorage;
    }

    public void OpenProductStorage()
    {
        UiManager.Instance.SetProductCapacity(BuildingStat.Effect_Value);
        UiManager.Instance.ShowProductsUi();
    }
}
