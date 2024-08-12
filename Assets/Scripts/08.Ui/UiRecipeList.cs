using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
