using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CraftingFloor : Floor
{
    public override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Start()
    {
        base.Start();
        UniAutoWork(cts.Token).Forget();
    }

    public override void LevelUp()
    {
        base.LevelUp();
    }

    public override void Set()
    {
        base.Set();
    }

    public override void RemoveAnimal(Animal animal)
    {
        base.RemoveAnimal(animal);
    }

    private async UniTaskVoid UniAutoWork(CancellationToken cts)
    {
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
                    if (!(b as CraftingBuilding).isCrafting)
                        continue;
                    if ((storage as StorageProduct).IsFull)
                    {
                        (b as CraftingBuilding).isCrafting = false; // 제작 끝
                        (b as CraftingBuilding).autoCrafting = false;
                        continue;
                    }

                    b.accumWorkLoad += autoWorkload;

                    while (b.accumWorkLoad >= (b as CraftingBuilding).recipeStat.Workload && ((b as CraftingBuilding).autoCrafting || (b as CraftingBuilding).amount >= 1))
                    {
                        if((storage as StorageProduct).IsFull || !(b as CraftingBuilding).autoCrafting)
                        {
                            (b as CraftingBuilding).isCrafting = false; // 제작 끝
                            break;
                        }
                        
                        // 생성
                        (storage as StorageProduct).IncreaseProduct((b as CraftingBuilding).recipeStat.Product_ID);

                        (b as CraftingBuilding).amount--;
                        b.accumWorkLoad = BigNumber.Zero;
                        if ((b as CraftingBuilding).amount != 0 || (b as CraftingBuilding).autoCrafting)
                        {
                            (b as CraftingBuilding).SetSlider();
                            break;
                        }

                        if((b as CraftingBuilding).autoCrafting)
                        {
                            (b as CraftingBuilding).amount = 1;
                        }

                        (b as CraftingBuilding).isCrafting = false; // 제작 끝
                    }

                    if ((storage as StorageProduct).IsFull)
                    {
                        (b as CraftingBuilding).isCrafting = false; // 제작 끝
                        (b as CraftingBuilding).autoCrafting = false;
                        continue;
                    }
                }
                NotifyObservers();
            }
        }
    }
}
