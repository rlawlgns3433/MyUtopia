using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiAnimalList : Observer
{
    public AnimalListMode mode = AnimalListMode.AnimalList;
    public List<UiAnimalFloorSlot> slots = new List<UiAnimalFloorSlot>();

    public GameObject animalInvenSlot;
    public List<UiFloorAnimal> parents = new List<UiFloorAnimal>();
    public Button buttonExchange;
    public Button buttonEliminate;

    public override void Notify(Subject subject)
    {
        foreach (var parent in parents)
        {
            foreach (var animal in parent.uiAnimalFloorSlots)
            {
                if (animal.animalClick == null)
                    return;
                animal.sliderStamina.value = animal.animalClick.AnimalWork.Animal.animalStat.Stamina;
            }
        }
    }

    //public UiAnimalFloorSlot SetAnimal(int floorId, AnimalClick animalClick)
    //{
    //    return parents[floorId - 1].Add(animalClick);
    //}

    public void MoveSlot(UiFloorAnimal from, UiFloorAnimal to, UiAnimalFloorSlot slot)
    {
        to.Add(slot.animalClick);
        from.Remove(slot);
        if (FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.MoveMurgeAnimal)
        {
            FloorManager.Instance.touchManager.tutorial.SetTutorialProgress();
        }
    }

    public void SetExchangeMode()
    {
        if (mode == AnimalListMode.Exchange)
        {
            mode = AnimalListMode.AnimalList;
            UiManager.Instance.IsAnimalList(true);
        }
        else
        {
            UiManager.Instance.OffAnimalList();
            mode = AnimalListMode.Exchange;
        }
    }

    public void SetEliminateMode()
    {
        if (mode == AnimalListMode.Eliminate)
        {
            mode = AnimalListMode.AnimalList;
            UiManager.Instance.IsAnimalList(true);
        }
        else
        {
            mode = AnimalListMode.Eliminate;
            UiManager.Instance.OffAnimalList();
        }
    }

    public void ExchangeAnimal()
    {
        string[] floors = new string[2];
        var length = slots.Count;

        for (int i = 0; i < length; ++i)
        {
            if (slots[i] == null)
                return;

            floors[i] = slots[i].animalClick.AnimalWork.Animal.animalStat.CurrentFloor;
        }

        if (!floors[0].Equals(floors[1]))
        {
            for (int i = 0; i < length; ++i)
            {
                var from = parents[int.Parse(floors[i].Substring(1)) - 1];
                var to = parents[int.Parse(floors[length - i - 1].Substring(1)) - 1];
                MoveSlot(from, to, slots[i]);

                GameManager.Instance.GetAnimalManager().MoveAnimal(floors[i], floors[length - i - 1], slots[i].animalClick.AnimalWork.Animal);
            }
        }

        slots.Clear();
    }

    public void EliminateAnimal()
    {
        var floor = slots[0].animalClick.AnimalWork.Animal.animalStat.CurrentFloor;

        var from = parents[int.Parse(floor.Substring(1)) - 1];
        var to = parents[0];

        MoveSlot(from, to, slots[0]);

        GameManager.Instance.GetAnimalManager().MoveAnimal(floor, "B1", slots[0].animalClick.AnimalWork.Animal);
        slots.Clear();
    }

    public void Refresh()
    {
        foreach(var parent in parents)
        {
            parent.Clear();
            parent.Refresh();
        }
    }
}
