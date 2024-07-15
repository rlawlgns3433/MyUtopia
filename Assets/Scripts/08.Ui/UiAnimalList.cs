using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class UiAnimalList : Observer
{
    public GameObject animalInvenSlot;
    public List<UiFloorAnimal> parents = new List<UiFloorAnimal>();

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

    public void SetAnimal(int floorId, AnimalClick animalClick)
    {
        parents[floorId - 1].Add(animalClick);
    }
}
