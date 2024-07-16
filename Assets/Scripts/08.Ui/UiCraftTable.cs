using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class UiCraftTable : MonoBehaviour
{
    public CraftingBuilding craftingBuilding;
    public UiCraftingSlot craftingPrefab;
    public UiRecipeSlot recipePrefab;

    public UiCraftingList uiCraftingList;
    public UiRecipeList uiRecipeList;

    private async void OnEnable()
    {
        await UniWaitBuildingTable();

        int level = craftingBuilding.BuildingStat.Level;
        var recipes = GetRecipes(level);

        for (int i = 0; i < recipes.Count; ++i)
        {
            uiRecipeList.Add(new RecipeStat(recipes[i]));
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < uiRecipeList.recipeSlots.Count; ++i)
        {
            Destroy(uiRecipeList.recipeSlots[i]);
        }
    }

    private List<int> GetRecipes(int buildingLevel)
    {
        var recipes = (from recipe in DataTableMgr.GetRecipeTable().GetKeyValuePairs.Values
        where recipe.Unlock_Lv <= buildingLevel
        select recipe.Recipe_ID).ToList();

        return recipes;
    }

    public async UniTask UniWaitBuildingTable()
    {
        while (!DataTableMgr.GetBuildingTable().IsLoaded)
        {
            await UniTask.Yield();
        }
        return;
    }
}
