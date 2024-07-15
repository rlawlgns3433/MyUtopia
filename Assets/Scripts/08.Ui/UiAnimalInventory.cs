using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiAnimalInventory : Observer
{
    public List<UiAnimalSlot> uiAnimalSlots = new List<UiAnimalSlot>();
    public UiAnimalSlot slotPrefab;
    public ScrollRect scrollRect;
    public int maxSlot = 5;
    public int currentIndex = 0;
    private Floor currentFloor;

    //private void Awake()
    //{
    //    UiAnimalSlot slot = Instantiate(slotPrefab, scrollRect.content);
    //    slot.SlotIndex = currentIndex++;
    //    uiAnimalSlots.Add(slot);
    //}

    public void SetFloor(Floor floor)
    {
        currentFloor = floor;
    }

    public override void Notify(Subject subject)
    {
        if (currentFloor.animals.Count >= currentFloor.FloorData.Max_Population)
            return;

        UiAnimalSlot slot = Instantiate(slotPrefab, scrollRect.content);
        var animalManager = GameObject.FindWithTag(Tags.AnimalManager).GetComponent<AnimalManager>();
        slot.SlotIndex = currentIndex++;
        animalManager.Create(currentFloor.transform.position, currentFloor, 10005001, slot.SlotIndex);
        uiAnimalSlots.Add(slot);
        uiAnimalSlots[currentIndex-1].SlotIndex = currentIndex-1;
    }
}
