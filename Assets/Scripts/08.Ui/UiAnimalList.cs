using System.Collections.Generic;
using UnityEngine;

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

    public UiAnimalFloorSlot SetAnimal(int floorId, AnimalClick animalClick)
    {
        return parents[floorId - 1].Add(animalClick);
    }

    public void MoveSlot(UiFloorAnimal from, UiFloorAnimal to, UiAnimalFloorSlot slot)
    {
        /*
        from에 있는 걸 지운다.
        to에 넣는다
         */
        to.Add(slot.animalClick);
        from.Remove(slot);
    }
}
