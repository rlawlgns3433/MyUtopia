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
    public List<UiCurrency> uiCurrencies = new List<UiCurrency>();
    public TextMeshProUGUI textWorkloadPerSec;
    private FloorData currentFloorData; // FloorData에 따라서 건물이 달라짐 프로퍼티로 구성 필요
    private FloorData nextFloorData;
    private CancellationTokenSource cts = new CancellationTokenSource();
    private BigInteger autoWorkload = 0;
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

    public BigInteger CostForLevelUp
    {
        get
        {
            return costForLevelUp.ToBigInteger();
        }

        set
        {
            costForLevelUp = BigIntegerExtensions.ToString(value);
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

    public bool LevelUp()
    {
        // 보유 재화 조건에 의해 레벨업

        ++CurrentLevel;
        levelUpEvent?.Invoke();

        return false;
    }

    private async UniTaskVoid UniAutoWork(CancellationToken cts)
    {
        while(true)
        {
            autoWorkload = BigInteger.Zero;

            foreach(var animal in animals)
            {
                autoWorkload += animal.Workload;
            }

            await UniTask.Delay(1000, cancellationToken: cts);
            if(!autoWorkload.IsZero)
            {
                workloadPerSec = BigIntegerExtensions.ToString(autoWorkload);
                textWorkloadPerSec.text = workloadPerSec;

                foreach(var b in buildings)
                {
                    if (b.BuildingData.Level == 0)
                        continue;
                    CurrencyManager.currency[(int)b.buildingType] += autoWorkload / b.BuildingData.Work_Require;
                }
            }
            else
            {
                textWorkloadPerSec.text = "0";
            }

            NotifyObservers();
        }
    }
}
