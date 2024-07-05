using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using TMPro;
using UnityEngine;

public class Floor : MonoBehaviour, IGrowable
{
    public LinkedList<Animal> animals = new LinkedList<Animal>();

    public TextMeshProUGUI textWorkloadPerSec;
    private FloorData currentFloorData;
    private FloorData nextFloorData;
    private CancellationTokenSource cts = new CancellationTokenSource();
    public BigInteger autoWorkload = 0;
    public string workloadPerSec;

    private int currentFloorId = 21501;
    private int CurrentFloorId
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

    private async void Start()
    {
        UniAutoWork(cts.Token).Forget();
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
                autoWorkload += animal.AutoHarvesting;
            }

            await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: cts);
            if(!autoWorkload.IsZero)
            {
                workloadPerSec = BigIntegerExtensions.ToString(autoWorkload);
                textWorkloadPerSec.text = workloadPerSec;
            }
        }
    }
}
