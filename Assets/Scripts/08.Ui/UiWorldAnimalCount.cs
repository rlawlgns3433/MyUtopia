using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiWorldAnimalCount : Observer
{
    private static readonly string formatWorldAnimals = "{0} / {1}";

    [SerializeField]
    private TextMeshProUGUI textWorldAnimals;

    public override void Notify(Subject subject)
    {
        int currentAnimalCount = 0;
        if (FloorManager.Instance.GetFloor("B1") == null)
            return;

        int maximumCount = FloorManager.Instance.GetFloor("B1").FloorStat.Max_Population;

        foreach (var floor in FloorManager.Instance.floors)
        {
            foreach (var animal in floor.Value.animals)
            {
                if (animal.animalWork == null)
                    continue;
                currentAnimalCount++;
            }
        }
        textWorldAnimals.text = string.Format(formatWorldAnimals, currentAnimalCount, maximumCount);
    }
}
