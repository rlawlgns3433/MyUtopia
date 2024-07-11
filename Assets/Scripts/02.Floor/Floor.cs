using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Floor : Subject, IGrowable
{
    public List<Animal> animals = new List<Animal>();
    public List<Building> buildings = new List<Building>();
    public List<Observer> uiCurrencies = new List<Observer>();
    [SerializeField]
    private int floorId;

    private FloorData floorData; // FloorData에 따라서 건물이 달라짐 프로퍼티로 구성 필요
    public FloorData FloorData
    {
        get
        {
            if (floorData.ID == 0)
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
    public StorageTest storage;
    private CancellationTokenSource cts = new CancellationTokenSource();
    private BigNumber autoWorkload;
    public string floorName;

    private void Start()
    {
        foreach (var c in uiCurrencies)
        {
            Attach(c);
        }
        UniAutoWork(cts.Token).Forget();
        FloorManager.AddFloor(floorName, this);
        UniSetBuilding().Forget();
    }

    public void LevelUp()
    {
        if (FloorData.Grade == FloorData.Grade_Max)
            return;

        // 필요 재화가 있는지 확인
        if (FloorData.Level_Up_Coin.ToBigNumber() > CurrencyManager.currency[(int)CurrencyType.Coin])
            return;
        if (FloorData.Level_Up_Resource_1 != 0)
        {
            if (FloorData.Resource_1_Value.ToBigNumber() > CurrencyManager.currency[FloorData.Level_Up_Resource_1])
                return;
        }

        if (FloorData.Level_Up_Resource_2 != 0)
        {
            if (FloorData.Resource_2_Value.ToBigNumber() > CurrencyManager.currency[FloorData.Level_Up_Resource_2])
                return;
        }

        if (FloorData.Level_Up_Resource_3 != 0)
        {
            if (FloorData.Resource_3_Value.ToBigNumber() > CurrencyManager.currency[FloorData.Level_Up_Resource_3])
                return;
        }

        CurrencyManager.currency[(int)CurrencyType.Coin] -= FloorData.Level_Up_Coin.ToBigNumber();
        CurrencyManager.currency[FloorData.Level_Up_Resource_1] -= FloorData.Level_Up_Resource_1 != 0 ? FloorData.Resource_1_Value.ToBigNumber() : BigNumber.Zero;
        CurrencyManager.currency[FloorData.Level_Up_Resource_2] -= FloorData.Level_Up_Resource_2 != 0 ? FloorData.Resource_2_Value.ToBigNumber() : BigNumber.Zero;
        CurrencyManager.currency[FloorData.Level_Up_Resource_3] -= FloorData.Level_Up_Resource_3 != 0 ? FloorData.Resource_3_Value.ToBigNumber() : BigNumber.Zero;

        FloorData = DataTableMgr.GetFloorTable().Get(floorData.ID + 1);
        Set();
    }

    public void RemoveAnimal(Animal animal)
    {
        if (animal == null)
            return;
        if (!animals.Contains(animal))
            return;

        animals.Remove(animal);
    }
    private async UniTaskVoid UniSetBuilding()
    {
        await UniTask.WaitUntil(() => storage != null && storage.Buildings != null && storage.Buildings.Length > 0);
        for (int i = 0; i < buildings.Count; i++)
        {
            storage.Buildings[i] = buildings[i];
        }
    }
    private async UniTaskVoid UniAutoWork(CancellationToken cts)
    {
        while (true)
        {
            autoWorkload = BigNumber.Zero;

            foreach (var animal in animals)
            {
                if (animal.Stamina <= 0)
                    autoWorkload += animal.animalData.Workload / 2;
                else
                    autoWorkload += animal.animalData.Workload;

                if (storage != null)
                    storage.CurrWorkLoad = autoWorkload;
            }


            await UniTask.Delay(1000, cancellationToken: cts);
            if (!autoWorkload.IsZero)
            {
                foreach (var b in buildings)
                {
                    if (b.isLock)
                        continue;
                    if (b.BuildingData.Level == 0)
                        continue;

                    b.accumWorkLoad += autoWorkload;
                    switch ((int)b.buildingType)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            if (b.accumWorkLoad > b.BuildingData.Work_Require)
                            {
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
                                CurrencyManager.currency[b.BuildingData.Materials_Type] -= c * b.BuildingData.Conversion_rate;
                                b.accumWorkLoad -= c * b.BuildingData.Work_Require;
                            }
                            else
                            {
                                b.accumWorkLoad += autoWorkload;
                            }
                            break;
                        case 7:
                            if (b.accumWorkLoad > b.BuildingData.Work_Require)
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

    public void Set()
    {
        foreach (var building in buildings)
        {
            if (floorData.Unlock_Content == building.BuildingData.ID)
            {
                building.isLock = false;
            }
        }
    }
}
