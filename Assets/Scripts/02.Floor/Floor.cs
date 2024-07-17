using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Floor : Subject, IGrowable
{
    public List<Animal> animals = new List<Animal>();
    public List<Building> buildings = new List<Building>();
    public List<Observer> uiCurrencies = new List<Observer>();
    [SerializeField]
    protected int floorId;

    protected FloorStat floorStat; // FloorData에 따라서 건물이 달라짐 프로퍼티로 구성 필요
    public FloorStat FloorStat
    {
        get
        {
            if (floorStat == null)
            {
                floorStat = new FloorStat(floorId);
            }
            return floorStat;
        }
        set
        {
            floorStat = value;
        }
    }
    public Storage storage;
    protected CancellationTokenSource cts = new CancellationTokenSource();
    protected BigNumber autoWorkload;
    public string floorName;

    public virtual async void OnEnable()
    {
        await UniWaitFloorTable();

        if (FloorStat.Floor_ID == 0)
        {
            FloorStat = new FloorStat(floorId);
        }
    }

    protected virtual void Start()
    {
        if(uiCurrencies.Count != 0)
        {
            foreach (var c in uiCurrencies)
            {
                Attach(c);
            }
        }

        FloorManager.Instance.AddFloor(floorName, this);
    }

    public virtual void LevelUp()
    {
        if (FloorStat.Grade == FloorStat.Grade_Max)
            return;

        // 필요 재화가 있는지 확인
        if (FloorStat.Level_Up_Coin_Value.ToBigNumber() > CurrencyManager.currency[CurrencyType.Coin])
            return;
        if (FloorStat.Level_Up_Resource_1 != 0)
        {
            if (FloorStat.Resource_1_Value.ToBigNumber() > CurrencyManager.currency[(CurrencyType)FloorStat.Level_Up_Resource_1])
                return;
        }

        if (FloorStat.Level_Up_Resource_2 != 0)
        {
            if (FloorStat.Resource_2_Value.ToBigNumber() > CurrencyManager.currency[(CurrencyType)FloorStat.Level_Up_Resource_2])
                return;
        }

        if (FloorStat.Level_Up_Resource_3 != 0)
        {
            if (FloorStat.Resource_3_Value.ToBigNumber() > CurrencyManager.currency[(CurrencyType)FloorStat.Level_Up_Resource_3])
                return;
        }

        CurrencyManager.currency[CurrencyType.Coin] -= FloorStat.Level_Up_Coin_Value.ToBigNumber();

        if(FloorStat.Level_Up_Resource_1 != 0)
        {
            CurrencyManager.currency[(CurrencyType)FloorStat.Level_Up_Resource_1] -= FloorStat.Resource_1_Value.ToBigNumber();
        }

        if (FloorStat.Level_Up_Resource_2 != 0)
        {
            CurrencyManager.currency[(CurrencyType)FloorStat.Level_Up_Resource_2] -= FloorStat.Resource_2_Value.ToBigNumber();
        }

        if (FloorStat.Level_Up_Resource_3 != 0)
        {
            CurrencyManager.currency[(CurrencyType)FloorStat.Level_Up_Resource_3] -= FloorStat.Resource_3_Value.ToBigNumber();
        }

        FloorStat = new FloorStat(floorStat.Floor_ID + 1);
        if(buildings.Count > 0)
        {
            Set();
        }
    }

    public virtual void RemoveAnimal(Animal animal)
    {
        if (animal == null)
            return;
        if (!animals.Contains(animal))
            return;

        animals.Remove(animal);
        Destroy(animal.animalWork.gameObject);
    }

    public virtual void Set()
    {
        foreach (var building in buildings)
        {
            if (floorStat.Unlock_Facility == building.BuildingStat.Building_ID)
            {
                building.isLock = false;
            }
        }
    }

    public void AttachObserver(Observer observer)
    {
        Attach(observer);
    }


    public async UniTask UniWaitFloorTable()
    {
        while(!DataTableMgr.GetFloorTable().IsLoaded)
        {
            await UniTask.Yield();
        }
        return;
    }
}
