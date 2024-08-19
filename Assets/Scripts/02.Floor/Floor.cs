using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Floor : Subject, IGrowable
{
    public List<Animal> AcquireSortedAnimal
    {
        get
        {
            List<Animal> sorted = new List<Animal>(animals);
            sorted.Sort((x, y) =>  x.animalStat.AcquireTime.CompareTo(y.animalStat.AcquireTime));
            return sorted;
        }
    }
    public List<Animal> WorkloadSortedAnimal
    {
        get
        {
            List<Animal> sorted = new List<Animal>(animals);
            sorted.Sort((x, y) => y.animalStat.Workload.CompareTo(x.animalStat.Workload));
            return sorted;
        }
    }
    public List<Animal> TypeSortedAnimal
    {
        get
        {
            List<Animal> sorted = new List<Animal>(animals);
            sorted.Sort((x, y) => x.animalStat.AnimalData.Animal_Type.CompareTo(y.animalStat.AnimalData.Animal_Type));
            return sorted;
        }
    }


    public List<Animal> animals = new List<Animal>();
    public List<Building> buildings = new List<Building>();
    public List<Observer> runtimeObservers = new List<Observer>();
    public List<SynergyStat> synergyStats = new List<SynergyStat>();
    public Furniture furniture;

    public bool IsUpgrading { get; set; }
    public float UpgradeStartTime { get; set; }


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
            if (buildings.Count > 0)
            {
                Set();
            }
        }
    }
    public Storage storage;
    protected CancellationTokenSource cts = new CancellationTokenSource();
    public BigNumber autoWorkload;
    public string floorName;
    private float offLineWorkLoad;
    public float OffLineWorkLoad
    {
        get
        {
            return offLineWorkLoad;
        }
        set
        {
            offLineWorkLoad = value;
        }
    }
    public Floor() { }
    public Floor(int floorId)
    {
        if(floorStat == null || floorStat.Floor_ID == 0)
        {
            floorStat = new FloorStat(floorId);
        }
    }


    public virtual async void OnEnable()
    {
        await UniWaitFloorTable();

        if (FloorStat.Floor_ID == 0)
        {
            FloorStat = new FloorStat(floorId);
        }

        FloorManager.Instance.AddFloor(floorName, this);
    }

    protected virtual void Start()
    {
        if(runtimeObservers.Count != 0)
        {
            foreach (var c in runtimeObservers)
            {
                Attach(c);
            }
        }
    }

    public virtual void LevelUp()
    {
        if (FloorStat.Grade == FloorStat.Grade_Max)
            return;

        FloorStat = new FloorStat(floorStat.Floor_ID + 1);
        furniture.Refresh();

        foreach (var animal in animals)
        {
            var controller = animal.animalWork.GetComponent<AnimalController>();
            controller.behaviorTreeRoot.InitializeBehaviorTree();
        }
    }

    public bool CheckCurrency()
    {
        // 필요 재화가 있는지 확인
        if (FloorStat.Level_Up_Coin_Value.ToBigNumber() > CurrencyManager.currency[CurrencyType.Coin])
            return false;

        if (FloorStat.Level_Up_Resource_1 != 0)
        {
            if (FloorStat.Resource_1_Value.ToBigNumber() > CurrencyManager.product[(CurrencyProductType)FloorStat.Level_Up_Resource_1])
                return false;
        }

        if (FloorStat.Level_Up_Resource_2 != 0)
        {
            if (FloorStat.Resource_2_Value.ToBigNumber() > CurrencyManager.product[(CurrencyProductType)FloorStat.Level_Up_Resource_2])
                return false;
        }

        if (FloorStat.Level_Up_Resource_3 != 0)
        {
            if (FloorStat.Resource_3_Value.ToBigNumber() > CurrencyManager.product[(CurrencyProductType)FloorStat.Level_Up_Resource_3])
                return false;
        }

        return true;
    }

    public void SpendCurrency()
    {
        CurrencyManager.currency[CurrencyType.Coin] -= FloorStat.Level_Up_Coin_Value.ToBigNumber();

        if (FloorStat.Level_Up_Resource_1 != 0)
        {
            CurrencyManager.product[(CurrencyProductType)FloorStat.Level_Up_Resource_1] -= FloorStat.Resource_1_Value.ToBigNumber();
        }

        if (FloorStat.Level_Up_Resource_2 != 0)
        {
            CurrencyManager.product[(CurrencyProductType)FloorStat.Level_Up_Resource_2] -= FloorStat.Resource_2_Value.ToBigNumber();
        }

        if (FloorStat.Level_Up_Resource_3 != 0)
        {
            CurrencyManager.product[(CurrencyProductType)FloorStat.Level_Up_Resource_3] -= FloorStat.Resource_3_Value.ToBigNumber();
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

    public virtual void RemoveAllAnimals()
    {
        foreach(var animal in animals)
        {
            Destroy(animal.animalWork.gameObject);
        }
        animals.Clear();
    }

    public virtual void Set()
    {
        foreach (var building in buildings)
        {
            if (floorStat.Unlock_Facility == building.BuildingStat.Building_ID)
            {
                building.gameObject.SetActive(true);
                building.BuildingStat.IsLock = false;
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
