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

        craftingBuilding = ClickableManager.CurrentClicked as CraftingBuilding;

        int level = craftingBuilding.BuildingStat.Level;
        var recipes = GetRecipes(level);

        // 제작 빌딩에서 제작중인 게 있다면 가져온다.
        if(craftingBuilding.isCrafting)
        {
            uiCraftingList.Add(craftingBuilding.recipeStat, craftingBuilding.autoCrafting, craftingBuilding.amount);
        }
        for (int i = 0; i < recipes.Count; ++i)
        {
            if(craftingBuilding.recipeStat != null && craftingBuilding.recipeStat.Recipe_ID == recipes[i] && craftingBuilding.isCrafting)
                continue;

            uiRecipeList.Add(new RecipeStat(recipes[i]), craftingBuilding.autoCrafting);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < uiRecipeList.recipeSlots.Count; ++i)
        {
            Destroy(uiRecipeList.recipeSlots[i]);
        }
        craftingBuilding = null;
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
