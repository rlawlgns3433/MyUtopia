using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BuildingFloor : Floor
{
    public List<SynergyStat> synergyStats = new List<SynergyStat>();

    protected override void Start()
    {
        base.Start();
        UniAutoWork(cts.Token).Forget();
    }

    private async UniTask UniAutoWork(CancellationToken cts)
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
            }

            // 시너지를 통해 업무량 증가 여부
            if (synergyStats.Count != 0)
            {
                int synergyValue = 0;
                foreach (var synergy in synergyStats)
                {
                    synergyValue += Mathf.FloorToInt(synergy.Synergy_Value * 100);
                }
                autoWorkload = autoWorkload + (autoWorkload * synergyValue) / 100;
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
