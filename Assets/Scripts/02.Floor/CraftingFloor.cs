using Cysharp.Threading.Tasks;
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
                    if ((storage as StorageProduct).IsFull)
                    {
                        (b as CraftingBuilding).isCrafting = false; // Á¦ÀÛ ³¡
                        continue;
                    }
                    if(!(b as CraftingBuilding).craftingSlider.gameObject.activeSelf)
                        (b as CraftingBuilding).craftingSlider.gameObject.SetActive(true);

                    b.accumWorkLoad += autoWorkload;

                    if(b.accumWorkLoad > (b as CraftingBuilding).CurrentRecipeStat.Workload)
                    {
                        (b as CraftingBuilding).FinishCrafting();
                        b.accumWorkLoad = BigNumber.Zero;
                    }
                }
                NotifyObservers();
            }
        }
    }

}
