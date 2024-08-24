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

    public void MoveSlot(UiFloorAnimal from, UiFloorAnimal to, UiAnimalFloorSlot slot)
    {
        to.Add(slot.animalClick);
        from.Remove(slot);
        if(FloorManager.Instance.touchManager.tutorial != null)
        {
            if (FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.MoveMurgeAnimal)
            {
                FloorManager.Instance.touchManager.tutorial.SetTutorialProgress();
            }
        }
    }

    public void SetAnimalListMode()
    {
        mode = AnimalListMode.AnimalList;

        slots.Clear();
        UiManager.Instance.IsAnimalList(true);
        foreach (var parent in parents)
        {
            foreach (var slot in parent.uiAnimalFloorSlots)
            {
                slot.imageExchange.gameObject.SetActive(false);
                slot.imageUnplace.gameObject.SetActive(false);
                slot.imagePortrait.color = Color.white;
            }
        }
    }

    public void SetExchangeMode()
    {
        if (mode == AnimalListMode.Exchange)
        {
            SetAnimalListMode();
        }
        else
        {
            UiManager.Instance.OffAnimalList();
            mode = AnimalListMode.Exchange;

            foreach (var parent in parents)
            {
                foreach (var slot in parent.uiAnimalFloorSlots)
                {
                    slot.imageExchange.gameObject.SetActive(true);
                    slot.imageUnplace.gameObject.SetActive(false);
                }
            }
        }
    }

    public void SetEliminateMode()
    {
        if (mode == AnimalListMode.Eliminate)
        {
            SetAnimalListMode();
        }
        else
        {
            UiManager.Instance.OffAnimalList();
            mode = AnimalListMode.Eliminate;

            foreach (var parent in parents)
            {
                foreach (var slot in parent.uiAnimalFloorSlots)
                {
                    slot.imageExchange.gameObject.SetActive(false);
                    slot.imageUnplace.gameObject.SetActive(true);
                }
            }
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

                GameManager.Instance.GetAnimalManager().MoveAnimal(floors[i], floors[length - i - 1], slots[i].animalClick.AnimalWork.Animal, true);
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

    public void Clear()
    {
        foreach(var parent in parents)
        {
            parent.Clear();
        }
    }

    public void Refresh()
    {
        foreach(var parent in parents)
        {
            parent.Clear();
            parent.Refresh();
        }
    }

    public void SortAnimal(AnimalSortType type)
    {
        foreach(var parent in parents)
        {
            parent.SortAnimal(type);
        }
    }
}
