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
                    b.accumWorkLoad += autoWorkload;

                    if (b.accumWorkLoad >= (b as CraftingBuilding).recipeStat.Workload)
                    {
                        // »ý¼º
                        (storage as StorageProduct).IncreaseProduct((b as CraftingBuilding).recipeStat.Product_ID);

                        //(b as CraftingBuilding).recipeStat.Product_ID

                        CurrencyManager.currency[CurrencyType.Craft] += 1;
                        (b as CraftingBuilding).isCrafting = false;
                        b.accumWorkLoad = BigNumber.Zero;
                    }
                }
                NotifyObservers();
            }
        }
    }
}
