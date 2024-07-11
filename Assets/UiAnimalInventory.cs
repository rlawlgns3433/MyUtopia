using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiAnimalInventory : MonoBehaviour
{
    public List<UiAnimalSlot> uiAnimalSlots = new List<UiAnimalSlot>();
    public UiAnimalSlot slotPrefab;
    public ScrollRect scrollRect;
    public int maxSlot = 5;

    private void Awake()
    {
        for (int i = 0; i < maxSlot + 1; ++i)
        {
            UiAnimalSlot slot = Instantiate(slotPrefab, scrollRect.content);
            slot.SlotIndex = i;
            slot.ClearData();
            uiAnimalSlots.Add(slot);
            if(i == maxSlot)
            {
                var button = slot.GetComponent<Button>();
                button.onClick.AddListener(OnClickAddAnimal);
            }
        }
    }

    public void OnClickAddAnimal()
    {
        foreach(var slot in uiAnimalSlots)
        {
            if(slot.IsEmpty)
            {
                var animalManager = GameObject.FindWithTag(Tags.AnimalManager).GetComponent<AnimalManager>();
                var floor = FloorManager.GetFloor("B5"); // 임시 코드
                animalManager.Create(floor.transform.position, floor, animalManager.hamsterPrefabReference, 0);
                break;
            }
        }
    }

    public void SetSlot(int slotId, AnimalClick animalClick)
    {
        uiAnimalSlots[slotId].SetData(animalClick);
    }
}
