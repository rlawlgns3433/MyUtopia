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

    protected FloorData floorData; // FloorData에 따라서 건물이 달라짐 프로퍼티로 구성 필요
    public FloorData FloorData
    {
        get
        {
            if (floorData.Floor_ID == 0)
            {
                floorData = DataTableMgr.GetFloorTable().Get(floorId);
            }
            return floorData;
        }
        set
        {
            floorData = value;
        }
    }
    public Storage storage;
    protected CancellationTokenSource cts = new CancellationTokenSource();
    protected BigNumber autoWorkload;
    public string floorName;

    public virtual void OnEnable()
    {
        if (floorData.Floor_ID == 0)
        {
            floorData = DataTableMgr.GetFloorTable().Get(floorId);
        }
    }

    protected virtual void Start()
    {
        foreach (var c in uiCurrencies)
        {
            Attach(c);
        }
        FloorManager.Instance.AddFloor(floorName, this);
    }

    public virtual void LevelUp()
    {
        if (FloorData.Grade == FloorData.Grade_Max)
            return;

        // 필요 재화가 있는지 확인
        if (FloorData.Level_Up_Coin_Value.ToBigNumber() > CurrencyManager.currency[CurrencyType.Coin])
            return;
        if (FloorData.Level_Up_Resource_1 != 0)
        {
            if (FloorData.Resource_1_Value.ToBigNumber() > CurrencyManager.currency[(CurrencyType)FloorData.Level_Up_Resource_1])
                return;
        }

        if (FloorData.Level_Up_Resource_2 != 0)
        {
            if (FloorData.Resource_2_Value.ToBigNumber() > CurrencyManager.currency[(CurrencyType)FloorData.Level_Up_Resource_2])
                return;
        }

        if (FloorData.Level_Up_Resource_3 != 0)
        {
            if (FloorData.Resource_3_Value.ToBigNumber() > CurrencyManager.currency[(CurrencyType)FloorData.Level_Up_Resource_3])
                return;
        }

        CurrencyManager.currency[CurrencyType.Coin] -= FloorData.Level_Up_Coin_Value.ToBigNumber();

        if(FloorData.Level_Up_Resource_1 != 0)
        {
            CurrencyManager.currency[(CurrencyType)FloorData.Level_Up_Resource_1] -= FloorData.Resource_1_Value.ToBigNumber();
        }

        if (FloorData.Level_Up_Resource_2 != 0)
        {
            CurrencyManager.currency[(CurrencyType)FloorData.Level_Up_Resource_2] -= FloorData.Resource_2_Value.ToBigNumber();
        }

        if (FloorData.Level_Up_Resource_3 != 0)
        {
            CurrencyManager.currency[(CurrencyType)FloorData.Level_Up_Resource_3] -= FloorData.Resource_3_Value.ToBigNumber();
        }

        FloorData = DataTableMgr.GetFloorTable().Get(floorData.Floor_ID + 1);
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
            if (floorData.Unlock_Facility == building.BuildingData.Building_ID)
            {
                building.isLock = false;
            }
        }
    }
}
