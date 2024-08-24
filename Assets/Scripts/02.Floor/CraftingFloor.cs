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

            // 시너지를 통해 업무량 증가 여부
            if (synergyStats.Count != 0)
            {
                int synergyValue = 0;
                foreach (var synergy in synergyStats)
                {
                    if(synergy.Synergy_Type == 1)
                    {
                        synergyValue += Mathf.FloorToInt(synergy.Synergy_Value * 100);
                    }
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
                        continue;
                    }

                    b.accumWorkLoad += autoWorkload;

                    while ((b as CraftingBuilding).CurrentRecipeStat != null && b.accumWorkLoad >= (b as CraftingBuilding).CurrentRecipeStat.Workload)
                    {
                        if ((storage as StorageProduct).IsFull)
                        {
                            (b as CraftingBuilding).isCrafting = false; // 제작 끝
                            break;
                        }
                        // 생성
                        (storage as StorageProduct).IncreaseProduct((b as CraftingBuilding).CurrentRecipeStat.Product_ID);
                        

                        (b as CraftingBuilding).CancelCrafting();

                        if((b as CraftingBuilding).recipeStatList.Count > 0)
                        {
                            (b as CraftingBuilding).CurrentRecipeStat = null;
                            (b as CraftingBuilding).Set((b as CraftingBuilding).recipeStatList.Peek());
                            UiManager.Instance.craftTableUi.RefreshAfterCrafting();
                            (b as CraftingBuilding).SetSlider();
                            break;
                        }

                        (b as CraftingBuilding).CurrentRecipeStat = null;
                        (b as CraftingBuilding).isCrafting = false; // 제작 끝
                        UiManager.Instance.craftTableUi.Refresh();
                    }

                    if ((storage as StorageProduct).IsFull)
                    {
                        (b as CraftingBuilding).isCrafting = false; // 제작 끝
                        continue;
                    }
                }
                NotifyObservers();
            }
        }
    }

}
