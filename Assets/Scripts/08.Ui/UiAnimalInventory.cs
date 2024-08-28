using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiAnimalInventory : MonoBehaviour
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

    private async void OnEnable()
    {
        await UniTask.WaitUntil(() => GameManager.Instance.isLoadedWorld);
        await SetOnEnable();
    }
    
    private async UniTask SetOnEnable()
    {
        currentIndex = 0;
        var animals = currentFloor.animals;

        foreach (var slot in uiAnimalSlots)
        {
            Destroy(slot.gameObject);
        }

        uiAnimalSlots = new List<UiAnimalSlot>();
        for (int j = animals.Count - 1; j >= 0; --j)
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
        await UniTask.Delay(100);
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

    //public override void Notify(Subject subject)
    //{
    //    if (currentFloor.animals.Count >= currentFloor.FloorStat.Max_Population)
    //        return;
    //    currentFloor = FloorManager.Instance.GetCurrentFloor();
    //    UiAnimalSlot slot = Instantiate(slotPrefab, scrollRect.content);
    //    var animalManager = GameObject.FindWithTag(Tags.AnimalManager).GetComponent<AnimalManager>();
    //    slot.SlotIndex = currentIndex++;
    //    animalManager.Create(currentFloor.transform.position, currentFloor, 10005001, slot.SlotIndex);
    //    uiAnimalSlots.Add(slot);
    //    uiAnimalSlots[currentIndex-1].SlotIndex = currentIndex-1;
    //}

    //public void SetAnimal()
    //{
    //    foreach (var slot in uiAnimalSlots)
    //    {
    //        if(slot.animalClick.AnimalWork.Animal.animalStat.CurrentFloor.Equals(currentFloor))
    //        {
    //            slot.SetData(slot.animalClick);
    //        }
    //    }
    //}

    public async void UpdateInventory(bool isMerged = false)
    {
        await UniTask.WaitUntil(() => GameManager.Instance.isLoadedWorld);
        await UniTask.WaitUntil(() => FloorManager.Instance != null);
        await InventorySetting();
    }

    private async UniTask InventorySetting()
    {
        if (FloorManager.Instance.CurrentFloorIndex <= maxSlot)
        {
            SetFloor(FloorManager.Instance.GetCurrentFloor());
            FloorManager.Instance.SetFloor(currentFloor.floorName);
        }

        for (int j = 0; j < uiAnimalSlots.Count; ++j)
        {
            uiAnimalSlots[j].ClearData();
        }
        uiAnimalSlots.Clear();
        currentIndex = 0;

        var animals = currentFloor.animals;
        uiAnimalSlots = new List<UiAnimalSlot>();
        for (int j = animals.Count - 1; j >= 0; --j)
        {
            if (j < 0)
                break;

            if (animals[j].animalWork == null)
            {
                animals.Remove(animals[j++]);
                continue;
            }

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
        await UniTask.Delay(100);
    }
}
