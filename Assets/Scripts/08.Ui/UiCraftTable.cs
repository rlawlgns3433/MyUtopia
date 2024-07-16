using UnityEngine;
using UnityEngine.UI;


public class UiCraftTable : MonoBehaviour
{
    public CraftingBuilding craftingBuilding;
    public UiCraftingSlot craftingPrefab;
    public UiRecipeSlot recipePrefab;

    public UiCraftingList uiCraftingList;
    public UiRecipeList uiRecipeList;

    private void OnEnable()
    {
        for (int i = 0; i < 3; ++i)
        {
            uiCraftingList.Add(new RecipeStat());
        }
    }
}
