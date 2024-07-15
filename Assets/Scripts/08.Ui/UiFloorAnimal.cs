using System.Collections.Generic;
using UnityEngine;

public class UiFloorAnimal : MonoBehaviour
{
    public UiAnimalFloorSlot slotPrefab;
    public List<UiAnimalFloorSlot> uiAnimalFloorSlots = new List<UiAnimalFloorSlot>();

    public void Add(AnimalClick animalClick)
    {
        var slot = Instantiate(slotPrefab, transform);
        slot.SetData(animalClick);
        uiAnimalFloorSlots.Add(slot);
    }
}
