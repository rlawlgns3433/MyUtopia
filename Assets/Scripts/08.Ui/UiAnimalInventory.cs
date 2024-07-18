using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiAnimalInventory : Observer
{
    public List<UiAnimalSlot> uiAnimalSlots;
    public UiAnimalSlot slotPrefab;
    public ScrollRect scrollRect;
    public int maxSlot = 5;
    public int currentIndex = 0;
    public Floor currentFloor;

    //private void Awake()
    //{
    //    UiAnimalSlot slot = Instantiate(slotPrefab, scrollRect.content);
    //    slot.SlotIndex = currentIndex++;
    //    uiAnimalSlots.Add(slot);
    //}

    private void OnEnable()
    {
        currentIndex = 0;
        var animals = currentFloor.animals;

        foreach(var slot in uiAnimalSlots)
        {
            Destroy(slot.gameObject);
        }

        uiAnimalSlots = new List<UiAnimalSlot>();
        for (int j = 0; j < animals.Count; ++j)
        {
            // Floor에 있는 동물 데이터를 여기서 담는다.
            UiAnimalSlot slot = Instantiate(slotPrefab, scrollRect.content);
            slot.SlotIndex = currentIndex++;
            slot.SetData(animals[j].animalWork.gameObject.GetComponent<AnimalClick>());
            uiAnimalSlots.Add(slot);
            uiAnimalSlots[currentIndex - 1].SlotIndex = currentIndex - 1;
            animals[j].animalWork.SetUiAnimalSlot(slot);
        }
        UiManager.Instance.mainUi.Refresh();
    }

    private void OnDisable()
    {
        for (int j = 0; j < uiAnimalSlots.Count; ++j)
        {
            uiAnimalSlots[j].ClearData();
        }
        uiAnimalSlots.Clear();
        currentIndex = 0;
    }

    public void SetFloor(Floor floor)
    {
        currentFloor = floor;
    }

    public override void Notify(Subject subject)
    {
        if (currentFloor.animals.Count >= currentFloor.FloorStat.Max_Population)
            return;
        currentFloor = FloorManager.Instance.GetCurrentFloor();
        UiAnimalSlot slot = Instantiate(slotPrefab, scrollRect.content);
        var animalManager = GameObject.FindWithTag(Tags.AnimalManager).GetComponent<AnimalManager>();
        slot.SlotIndex = currentIndex++;
        animalManager.Create(currentFloor.transform.position, currentFloor, 10005001, slot.SlotIndex);
        uiAnimalSlots.Add(slot);
        uiAnimalSlots[currentIndex-1].SlotIndex = currentIndex-1;
    }

    public void SetAnimal()
    {
        foreach (var slot in uiAnimalSlots)
        {
            if(slot.animalClick.AnimalWork.Animal.animalStat.CurrentFloor.Equals(currentFloor))
            {
                slot.SetData(slot.animalClick);
            }
        }
    }

    public void UpdateInventory(bool isMerged)
    {
        if (isMerged)
            return;
        for (int j = 0; j < uiAnimalSlots.Count; ++j)
        {
            uiAnimalSlots[j].ClearData();
        }
        uiAnimalSlots.Clear();
        currentIndex = 0;

        var animals = currentFloor.animals;
        uiAnimalSlots = new List<UiAnimalSlot>();
        for (int j = 0; j < animals.Count; ++j)
        {
            if (animals[j].animalWork.gameObject.GetComponent<AnimalClick>() == null)
                continue;

            // Floor에 있는 동물 데이터를 여기서 담는다.
            UiAnimalSlot slot = Instantiate(slotPrefab, scrollRect.content);
            slot.SlotIndex = currentIndex++;
            slot.SetData(animals[j].animalWork.gameObject.GetComponent<AnimalClick>());
            uiAnimalSlots.Add(slot);
            uiAnimalSlots[currentIndex - 1].SlotIndex = currentIndex - 1;
            animals[j].animalWork.SetUiAnimalSlot(slot);
        }

        UiManager.Instance.mainUi.Refresh();
    }
}
