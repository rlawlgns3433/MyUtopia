using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class UiCraftTable : MonoBehaviour
{
    public CraftingBuilding craftingBuilding;
    public UiRecipeSlot recipePrefab;

    public UiCraftingSlot uiCraftingSlot;
    public UiRecipeList uiRecipeList;



    //private async void OnEnable()
    //{
    //    await UniWaitBuildingTable();

    //    craftingBuilding = ClickableManager.CurrentClicked as CraftingBuilding;

    //    int level = craftingBuilding.BuildingStat.Level;
    //    var recipes = GetRecipes(level);

    //    // 제작 빌딩에서 제작중인 게 있다면
    //    if(craftingBuilding.isCrafting)
    //    {
    //        // 제작중인 것을 불러온다
    //        // 대기큐에 걸려 있는 것을 불러온다.
    //        uiCraftingList.Add(craftingBuilding.recipeStat, craftingBuilding.amount);
    //    }
    //    for (int i = 0; i < recipes.Count; ++i)
    //    {
    //        if(craftingBuilding.recipeStat != null && craftingBuilding.recipeStat.Recipe_ID == recipes[i] && craftingBuilding.isCrafting)
    //            continue;

    //        uiRecipeList.Add(new RecipeStat(recipes[i]));
    //    }
    //}

    //private void OnDisable()
    //{
    //    foreach(var slot in uiRecipeList.recipeSlots)
    //    {
    //        Destroy(slot.gameObject);
    //    }
    //    uiRecipeList.recipeSlots.Clear();
    //    craftingBuilding = null;
    //}

    private async void OnEnable()
    {
        await UniWaitBuildingTable();

        craftingBuilding = ClickableManager.CurrentClicked as CraftingBuilding;

        int level = craftingBuilding.BuildingStat.Level;
        var recipes = GetRecipes(level);

        for (int i = 0; i < recipes.Count; ++i)
        {
            uiRecipeList.Add(new RecipeStat(recipes[i]));
        }

        Refresh();

        //foreach (var slot in uiCraftingSlot.waitingSlots)
        //{
        //    slot.ClearData();
        //}

        //if (craftingBuilding.recipeStatList.Count > 0)
        //{
        //    uiCraftingSlot.recipeCurrentCrafting = craftingBuilding.recipeStatList.Peek();
        //    uiCraftingSlot.imageCurrentCrafting.sprite = await uiCraftingSlot.recipeCurrentCrafting.RecipeData.GetProduct().GetImage();
        //}

        //if(craftingBuilding.recipeStatList.Count > 0)
        //{
        //    Queue<RecipeStat> temp = new Queue<RecipeStat>(craftingBuilding.recipeStatList);
        //    temp.Dequeue();

        //    for (int i = 0; i < craftingBuilding.recipeStatList.Count; ++i)
        //    {
        //        uiCraftingSlot.waitingSlots[i].SetData(temp.Dequeue());
        //    }
        //}
    }

    private void OnDisable()
    {
        foreach (var slot in uiRecipeList.recipeSlots)
        {
            Destroy(slot.gameObject);
        }
        uiRecipeList.recipeSlots.Clear();
        //craftingBuilding = null;
    }

    private List<int> GetRecipes(int buildingLevel)
    {
        var recipes = (from recipe in DataTableMgr.GetRecipeTable().GetKeyValuePairs.Values
                       where recipe.Unlock_Lv <= buildingLevel
                       select recipe.Recipe_ID).ToList();

        return recipes;
    }

    public async void Refresh()
    {
        foreach (var slot in uiCraftingSlot.waitingSlots)
        {
            slot.ClearData();
        }

        if (craftingBuilding.recipeStatList.Count > 0)
        {
            Queue<RecipeStat> temp = new Queue<RecipeStat>(craftingBuilding.recipeStatList);

            uiCraftingSlot.recipeCurrentCrafting = craftingBuilding.CurrentRecipeStat;
            uiCraftingSlot.imageCurrentCrafting.sprite = await uiCraftingSlot.recipeCurrentCrafting.RecipeData.GetProduct().GetImage();
            uiCraftingSlot.SetData(uiCraftingSlot.recipeCurrentCrafting);

            int i = 0;
            while (temp.Count > 0)
            {
                uiCraftingSlot.waitingSlots[i++].SetData(temp.Dequeue());
            }
        }
    }

    public async void RefreshAfterCrafting()
    {
        foreach (var slot in uiCraftingSlot.waitingSlots)
        {
            slot.ClearData();
        }

        if (craftingBuilding.recipeStatList.Count > 0)
        {
            Queue<RecipeStat> temp = new Queue<RecipeStat>(craftingBuilding.recipeStatList);

            uiCraftingSlot.recipeCurrentCrafting = temp.Peek();
            uiCraftingSlot.imageCurrentCrafting.sprite = await temp.Dequeue().RecipeData.GetProduct().GetImage();
            uiCraftingSlot.SetData(uiCraftingSlot.recipeCurrentCrafting);

            craftingBuilding.recipeStatList.Dequeue();

            int i = 0;
            while(temp.Count > 0)
            {
                uiCraftingSlot.waitingSlots[i++].SetData(temp.Dequeue());
            }
        }
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
