using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiFloorAnimal : MonoBehaviour
{
    public AnimalSortType sortType = AnimalSortType.Default;
    public UiAnimalFloorSlot slotPrefab;
    public List<UiAnimalFloorSlot> uiAnimalFloorSlots = new List<UiAnimalFloorSlot>();
    public Floor floor;

    private void OnEnable()
    {
        SortAnimal(sortType);
    }

    private void OnDisable()
    {
        var animals = floor.animals;
        Clear();
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
        //slot.GetComponent<Transform>().SetAsFirstSibling(); // 가장 처음 나타나도록

        return slot;
    }

    public void Remove(UiAnimalFloorSlot slot)
    {
        if(uiAnimalFloorSlots.Contains(slot))
        {
            uiAnimalFloorSlots.Remove(slot);
            Destroy(slot);
            Destroy(slot.gameObject);
        }
    }

    public void Clear()
    {
        foreach(var slot in uiAnimalFloorSlots)
        {
            if(slot == null)
            {
                uiAnimalFloorSlots.Remove(slot);
                continue;
            }

            if (slot.gameObject == null)
                continue;

            Destroy(slot);
            Destroy(slot.gameObject);
        }
        uiAnimalFloorSlots.Clear();
    }

    public void Refresh()
    {
        var animals = floor.animals;

        Clear();

        for (int j = 0; j < animals.Count; ++j)
        {
            var animalClick = animals[j].animalWork.gameObject.GetComponent<AnimalClick>();
            if (animalClick == null)
                continue;
            animals[j].animalWork.uiAnimalFloorSlot = Add(animalClick);
            animals[j].animalWork.SetUiAnimalFloorSlot(animals[j].animalWork.uiAnimalFloorSlot);
        }
    }

    public void SortAnimal(AnimalSortType type)
    {
        List<Animal> animals = null;
        sortType = type;

        switch (sortType)
        {
            case AnimalSortType.Default:
                animals = floor.animals;
                break;
            case AnimalSortType.Acquire:
                animals = floor.AcquireSortedAnimal;
                break;
            case AnimalSortType.Workload:
                animals = floor.WorkloadSortedAnimal;
                break;
            case AnimalSortType.Type:
                animals = floor.TypeSortedAnimal;
                break;
        }

        if (animals == null)
            return;

        for (int j = 0; j < animals.Count; ++j)
        {
            if (animals[j].animalWork == null || animals[j] == null)
                continue;
            var animalClick = animals[j].animalWork.gameObject.GetComponent<AnimalClick>();
            animals[j].animalWork.uiAnimalFloorSlot = Add(animalClick);
            animals[j].animalWork.SetUiAnimalFloorSlot(animals[j].animalWork.uiAnimalFloorSlot);
        }
    }
}
