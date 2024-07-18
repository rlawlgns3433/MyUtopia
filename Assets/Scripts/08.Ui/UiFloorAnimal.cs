using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        var button = slot.gameObject.GetComponent<Button>();
        if (FloorManager.Instance.GetCurrentFloor() == floor)
        {
            if (UiManager.Instance.isAnimalMove)
            {
                button.interactable = false;
            }
        }
        else
        {
            button.interactable = true;
        }

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

    public void Refresh()
    {
        var animals = floor.animals;

        for (int j = 0; j < animals.Count; ++j)
        {
            Clear();
        }

        for (int j = 0; j < animals.Count; ++j)
        {
            var animalClick = animals[j].animalWork.gameObject.GetComponent<AnimalClick>();
            animals[j].animalWork.uiAnimalFloorSlot = Add(animalClick);
            animals[j].animalWork.SetUiAnimalFloorSlot(animals[j].animalWork.uiAnimalFloorSlot);
        }
    }
}
