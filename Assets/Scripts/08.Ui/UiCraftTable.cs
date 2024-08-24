using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UiCraftTable : MonoBehaviour
{
    public CraftingBuilding craftingBuilding;
    public UiRecipeList uiRecipeList;
    public UiCraftingSlot uiCraftingSlot;

    private void OnEnable()
    {
        uiRecipeList.Clear();

        this.craftingBuilding = ClickableManager.CurrentClicked as CraftingBuilding;
        var recipes = GetRecipes(craftingBuilding.BuildingStat.Level);

        foreach(var recipe in recipes)
        {
            var recipeStat = new RecipeStat(recipe);
            uiRecipeList.Add(recipeStat);
        }

        uiCraftingSlot.SetData(craftingBuilding);
    }

    private List<int> GetRecipes(int buildingLevel)
    {
        var recipes = (from recipe in DataTableMgr.GetRecipeTable().GetKeyValuePairs.Values
                       where recipe.Unlock_Lv <= buildingLevel
                       select recipe.Recipe_ID).ToList();

        return recipes;
    }
}