using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiCraftingList : MonoBehaviour
{
    public UiCraftingSlot slotPrefab;
    public List<UiCraftingSlot> craftingSlots = new List<UiCraftingSlot>();

    private void OnDisable()
    {
        for (int i = 0; i < craftingSlots.Count; ++i)
        {
            Destroy(craftingSlots[i]);
        }
    }

    public UiCraftingSlot Add(RecipeStat recipeStat) // 레시피로 변경
    {
        var slot = Instantiate(slotPrefab, transform);
        slot.SetData(recipeStat);
        craftingSlots.Add(slot);

        return slot;
    }
}
