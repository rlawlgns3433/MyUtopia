using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class FacilityFloor : Floor
{
    protected override void Start()
    {
        base.Start();
        UniAutoHeal(cts.Token).Forget();
    }
    private async UniTaskVoid UniAutoHeal(CancellationToken cts)
    {
        Debug.Log("facilityFloorInit");
        while (true)
        {
            autoWorkload = BigNumber.Zero;

            foreach (var animal in animals)
            {
                animal.animalStat.CurrentFloor = floorName;
            }
            await UniTask.Delay(1000, cancellationToken: cts);
            NotifyObservers();
        }
    }
}
