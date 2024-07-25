using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BuildingFloor : Floor
{
    protected override void Start()
    {
        base.Start();
        UniAutoWork(cts.Token).Forget();
        UniSetBuilding().Forget();
    }

    private async UniTaskVoid UniSetBuilding()
    {
        var storageConduct = storage as StorageConduct;

        await UniTask.WaitUntil(() => storageConduct != null && storageConduct.Buildings != null && storageConduct.Buildings.Length > 0);
        for (int i = 0; i < buildings.Count; i++)
        {
            storageConduct.Buildings[i] = buildings[i];
        }
    }
    private async UniTaskVoid UniAutoWork(CancellationToken cts)
    {
        var storageConduct = storage as StorageConduct;

        while (true)
        {
            autoWorkload = BigNumber.Zero;

            foreach (var animal in animals)
            {
                if (animal.animalStat.Stamina <= 0)
                    autoWorkload += new BigNumber(animal.animalStat.Workload) / 2;
                else
                    autoWorkload += new BigNumber(animal.animalStat.Workload);

                if (storage != null)
                    storageConduct.CurrWorkLoad = autoWorkload;
            }


            await UniTask.Delay(1000, cancellationToken: cts);
            if (!autoWorkload.IsZero)
            {
                foreach (var b in buildings)
                {
                    if (b.BuildingStat.IsLock)
                        continue;
                    if (b.BuildingStat.Level == 0)
                        continue;

                    b.accumWorkLoad += autoWorkload;
                    switch (b.buildingType)
                    {
                        case CurrencyType.Coin:
                        case CurrencyType.CopperStone:
                        case CurrencyType.SilverStone:
                        case CurrencyType.GoldStone:
                            if (b.accumWorkLoad > b.BuildingStat.Work_Require)
                            {
                                BigNumber c = b.accumWorkLoad / b.BuildingStat.Work_Require;
                                CurrencyManager.currency[b.buildingType] += c;
                                //b.accumWorkLoad = b.accumWorkLoad - c * b.BuildingStat.Work_Require; 기존
                                b.accumWorkLoad = BigNumber.Zero;

                                var pos = b.transform.position;
                                pos.y += 1;

                                DynamicTextManager.CreateText(pos, c.ToString(), DynamicTextManager.autoWorkData, 2, 0.5f);
                            }
                            else
                                b.accumWorkLoad += autoWorkload;

                            break;
                        case CurrencyType.CopperIngot:
                        case CurrencyType.SilverIngot:
                        case CurrencyType.GoldIngot:
                            if (b.accumWorkLoad > b.BuildingStat.Work_Require)
                            {
                                if (CurrencyManager.currency[(CurrencyType)b.BuildingStat.Materials_Type] < b.BuildingStat.Conversion_rate)
                                {
                                    b.accumWorkLoad = BigNumber.Zero;
                                    break;
                                }

                                BigNumber c = b.accumWorkLoad / b.BuildingStat.Work_Require;

                                var temp = CurrencyManager.currency[(CurrencyType)b.BuildingStat.Materials_Type];

                                CurrencyManager.currency[(CurrencyType)b.BuildingStat.Materials_Type] -= c * b.BuildingStat.Conversion_rate; // 뺀 후의 값

                                if(temp < CurrencyManager.currency[(CurrencyType)b.BuildingStat.Materials_Type])
                                {
                                    CurrencyManager.currency[(CurrencyType)b.BuildingStat.Materials_Type] = temp;

                                    b.accumWorkLoad = BigNumber.Zero;

                                    break;
                                }

                                CurrencyManager.currency[b.buildingType] += c;
                                //b.accumWorkLoad -= c * b.BuildingStat.Work_Require; 기존
                                b.accumWorkLoad = BigNumber.Zero;
                            }
                            else
                            {
                                b.accumWorkLoad += autoWorkload;
                            }
                            break;
                    }
                }
            }
            NotifyObservers();
        }
    }
}
