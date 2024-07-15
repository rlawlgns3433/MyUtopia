using System.Collections.Generic;
using UnityEngine;

public class UiAnimalList : Observer
{
    public GameObject animalInvenSlot;
    public List<UiFloorAnimal> parents = new List<UiFloorAnimal>();

    private void OnEnable()
    {
        var floors = FloorManager.Instance.floors;

        for (int i = 0; i < floors.Count + 1; ++i)
        {
            if (i <= 2)
                continue;
            var animals = floors[$"B{i + 1}"].animals;
            for (int j = 0; j < animals.Count; ++j)
            {
                var animalClick = animals[j].animalWork.gameObject.GetComponent<AnimalClick>();
                animals[j].animalWork.uiAnimalFloorSlot = parents[i].Add(animalClick);
                animals[j].animalWork.SetUiAnimalFloorSlot(animals[j].animalWork.uiAnimalFloorSlot);
            }
        }
    }

    private void OnDisable()
    {
        var floors = FloorManager.Instance.floors;

        for (int i = 0; i < floors.Count + 1; ++i)
        {
            if (i <= 2)
                continue;
            for (int j = 0; j < floors[$"B{i + 1}"].animals.Count; ++j)
            {
                parents[i].Clear();
            }
        }
    }

    public override void Notify(Subject subject)
    {
        foreach(var parent in parents)
        {
            foreach(var animal in parent.uiAnimalFloorSlots)
            {
                if (animal.animalClick == null)
                    return;
                animal.sliderStamina.value = animal.animalClick.AnimalWork.Animal.animalStat.Stamina;
            }
        }
    }

    public UiAnimalFloorSlot SetAnimal(int floorId, AnimalClick animalClick)
    {
        return parents[floorId - 1].Add(animalClick);
    }
}
