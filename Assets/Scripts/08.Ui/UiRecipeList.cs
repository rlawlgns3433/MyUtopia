using System.Collections.Generic;
using UnityEngine;

public class UiRecipeList : MonoBehaviour
{
    public UiRecipeSlot slotPrefab;
    public List<UiRecipeSlot> recipeSlots = new List<UiRecipeSlot>();

    public UiRecipeSlot Add(RecipeStat recipeStat) // 레시피로 변경
    {
        var slot = Instantiate(slotPrefab, transform);
        slot.SetData(recipeStat);
        recipeSlots.Add(slot);

        return slot;
    }

    public void Clear()
    {
        foreach (var slot in recipeSlots)
        {
            Destroy(slot.gameObject);
        }

        recipeSlots.Clear();
    }
}
