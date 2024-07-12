using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiAnimalInventory : MonoBehaviour
{
    public List<UiAnimalSlot> uiAnimalSlots = new List<UiAnimalSlot>();
    public UiAnimalSlot slotPrefab;
    public ScrollRect scrollRect;
    public int maxSlot = 5;
    public int currentIndex = 0;

    private void Awake()
    {
        UiAnimalSlot slot = Instantiate(slotPrefab, scrollRect.content);
        slot.SlotIndex = currentIndex++;
        var button = slot.GetComponent<Button>();
        button.onClick.AddListener(OnClickAddAnimal);
        uiAnimalSlots.Add(slot);
    }

    public void OnClickAddAnimal()
    {
        UiAnimalSlot slot = Instantiate(slotPrefab, scrollRect.content);
        var animalManager = GameObject.FindWithTag(Tags.AnimalManager).GetComponent<AnimalManager>();
        var floor = FloorManager.Instance.GetFloor("B5"); // 임시 코드
        animalManager.Create(floor.transform.position, floor, 10005001, slot.SlotIndex);


        if (floor.animals.Count >= floor.FloorData.Max_Population)
            return;

        uiAnimalSlots.Add(slot);
        slot.SlotIndex = currentIndex++;
        uiAnimalSlots[currentIndex - 1].SlotIndex = currentIndex - 1;
    }
}
