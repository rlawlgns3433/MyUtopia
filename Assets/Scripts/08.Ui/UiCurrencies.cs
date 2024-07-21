using TMPro;
using UnityEngine;

public class UiCurrencies : MonoBehaviour
{
    private static readonly string formatWorldAnimals = "{0} / {1}";

    [SerializeField]
    private TextMeshProUGUI textWorldAnimals;

    public void SetAllAnimals()
    {
        int currentAnimalCount = 0;
        int maximumCount = 0;

        foreach(var floor in FloorManager.Instance.floors)
        {
            maximumCount += floor.Value.FloorStat.Max_Population;
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
