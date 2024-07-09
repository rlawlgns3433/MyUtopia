using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using TMPro;
using UnityEngine;

public class Floor : Subject, IGrowable
{
    public List<Animal> animals = new List<Animal>();
    public List<Building> buildings = new List<Building>();
    public List<Observer> uiCurrencies = new List<Observer>();
    private FloorData currentFloorData; // FloorData에 따라서 건물이 달라짐 프로퍼티로 구성 필요
    private FloorData nextFloorData;
    private CancellationTokenSource cts = new CancellationTokenSource();
    private BigNumber autoWorkload;
    private string workloadPerSec;
    public string floorName;

    private int currentFloorId = 21501;
    public int CurrentFloorId
    {
        get
        {
            return currentFloorId;
        }
        set
        {
            currentFloorId = value;
            currentFloorData = DataTableMgr.Get<FloorTable>(DataTableIds.Floor).Get(currentFloorId);
        }
    }

    [SerializeField]
    private int currentLevel;
    public int CurrentLevel
    {
        get
        {
            return currentLevel;
        }

        set
        {
            // Floor Data 교체
            currentLevel = value;
            currentFloorData = nextFloorData;
        }
    }
    [SerializeField]
    private int maxLevel;
    public int MaxLevel { get => currentFloorData.Grade_Max; }
    [SerializeField]
    private string costForLevelUp;

    public event Action levelUpEvent;

    public BigNumber CostForLevelUp
    {
        get
        {
            return new BigNumber(costForLevelUp);
        }

        set
        {
            costForLevelUp = value.ToString();
        }
    }

    private void Awake()
    {
        //currentFloorData = DataTableMgr.Get<FloorTable>(DataTableIds.Floor).Get(CurrentFloorId);
        //nextFloorData = DataTableMgr.Get<FloorTable>(DataTableIds.Floor).Get(++CurrentFloorId);
    }

    private void Start()
    {
        foreach(var c in uiCurrencies)
        {
            Attach(c);
        }
        UniAutoWork(cts.Token).Forget();
        FloorManager.AddFloor(floorName, this);
    }

    public void LevelUp()
    {
        // 보유 재화 조건에 의해 레벨업

        ++CurrentLevel;
        levelUpEvent?.Invoke();
    }

    public void RemoveAnimal(Animal animal)
    {
        if (animal == null) 
            return;
        if (!animals.Contains(animal))
            return;

        animals.Remove(animal);
    }

    private async UniTaskVoid UniAutoWork(CancellationToken cts)
    {
        while(true)
        {
            autoWorkload = BigNumber.Zero;

            foreach(var animal in animals)
            {
                if (animal.Stamina <= 0)
                    autoWorkload += animal.animalData.Workload / 2;
                else 
                    autoWorkload += animal.animalData.Workload;
            }


            await UniTask.Delay(1000, cancellationToken: cts);
            if(!autoWorkload.IsZero)
            {
                Debug.Log(autoWorkload.ToSimpleString());

                foreach (var b in buildings)
                {
                    if (b.BuildingData.Level == 0)
                        continue;

                    b.accumWorkLoad += autoWorkload;

                    switch ((int)b.buildingType)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            if(b.accumWorkLoad > b.BuildingData.Work_Require)
                            {
                                //CurrencyManager.currency[(int)b.buildingType] += b.accumWorkLoad / b.BuildingData.Work_Require;
                                //b.accumWorkLoad = b.accumWorkLoad % b.BuildingData.Work_Require;
                                BigNumber c = b.accumWorkLoad / b.BuildingData.Work_Require;
                                CurrencyManager.currency[(int)b.buildingType] += c;
                                b.accumWorkLoad = b.accumWorkLoad - c * b.BuildingData.Work_Require;
                            }
                            else
                                b.accumWorkLoad += autoWorkload;

                            break;
                        case 4:
                        case 5:
                        case 6:
                            if (b.accumWorkLoad > b.BuildingData.Work_Require)
                            {
                                if (CurrencyManager.currency[b.BuildingData.Materials_Type] < b.BuildingData.Conversion_rate)
                                {
                                    b.accumWorkLoad = new BigNumber(0);
                                    break;
                                }

                                BigNumber c = b.accumWorkLoad / b.BuildingData.Work_Require;
                                CurrencyManager.currency[(int)b.buildingType] += c;
                                CurrencyManager.currency[(int)b.buildingType - 3] -= c * b.BuildingData.Conversion_rate;
                                b.accumWorkLoad -= c * b.BuildingData.Work_Require;
                            }
                            else
                            {
                                b.accumWorkLoad += autoWorkload;
                            }
                            break;
                        case 7:
                            if(b.accumWorkLoad > b.BuildingData.Work_Require)
                            {
                                // 레시피 정보 불러오기
                                if (CurrencyManager.currency[(int)CurrencyType.Coin] > 10 && CurrencyManager.currency[(int)CurrencyType.CopperStone] > 10)
                                {
                                    CurrencyManager.currency[(int)CurrencyType.Coin] -= 10;
                                    CurrencyManager.currency[(int)CurrencyType.CopperStone] -= 10;
                                    CurrencyManager.currency[(int)CurrencyType.Craft] += 1;

                                    b.accumWorkLoad -= b.BuildingData.Work_Require;
                                }
                            }
                            else
                            {
                                b.accumWorkLoad += autoWorkload;
                            }
                            break;
                    }
                    NotifyObservers();
                }
            }
        }
    }
}
