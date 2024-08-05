using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiCraftingList : MonoBehaviour
{
    public UiCraftingSlot slotPrefab;
    public List<UiCraftingSlot> craftingSlots = new List<UiCraftingSlot>();
    public bool autoCraft = false;

    private void OnDisable()
    {
        for (int i = 0; i < craftingSlots.Count; ++i)
        {
            Destroy(craftingSlots[i]);
        }
        craftingSlots.Clear();
        autoCraft = false;
    }

    public UiCraftingSlot Add(RecipeStat recipeStat, int amount = 1, bool autoCraft = false) // 레시피로 변경
    {
        //if(autoCraft) // 이상
        //    return null;

        var slot = Instantiate(slotPrefab, transform);
        slot.SetData(recipeStat, autoCraft, amount);
        craftingSlots.Add(slot);

        return slot;
    }
}
