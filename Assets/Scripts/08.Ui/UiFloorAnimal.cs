using System.Collections.Generic;
using UnityEngine;

public class UiFloorAnimal : MonoBehaviour
{
    public UiAnimalFloorSlot slotPrefab;
    public List<UiAnimalFloorSlot> uiAnimalFloorSlots = new List<UiAnimalFloorSlot>();
    public Floor floor;

    private void OnEnable()
    {
        var animals = floor.animals;

        for (int j = 0; j < animals.Count; ++j)
        {
            var animalClick = animals[j].animalWork.gameObject.GetComponent<AnimalClick>();
            animals[j].animalWork.uiAnimalFloorSlot = Add(animalClick);
            animals[j].animalWork.SetUiAnimalFloorSlot(animals[j].animalWork.uiAnimalFloorSlot);
        }
    }

    private void OnDisable()
    {
        var animals = floor.animals;

        for (int j = 0; j < animals.Count; ++j)
        {
            Clear();
        }
    }

    public UiAnimalFloorSlot Add(AnimalClick animalClick)
    {
        var slot = Instantiate(slotPrefab, transform);
        slot.SetData(animalClick);
        uiAnimalFloorSlots.Add(slot);

        return slot;
    }

    public void Remove(UiAnimalFloorSlot slot)
    {
        if(uiAnimalFloorSlots.Contains(slot))
            Destroy(slot.gameObject);
    }

    public void Clear()
    {
        foreach(var slot in uiAnimalFloorSlots)
        {
            if (slot.gameObject == null)
                continue;

            Destroy(slot.gameObject);
        }
        uiAnimalFloorSlots.Clear();
    }
}
