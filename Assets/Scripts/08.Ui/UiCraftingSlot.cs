using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class UiCraftingSlot : Observer
{
    public CraftingBuilding craftingBuilding;
    public Image imageCurrentSlot;
    public TextMeshProUGUI textCurrentSlot;
    public List<Image> imagesWaiting = new List<Image>();
    public Slider sliderProcess;
    public Button buttonAccelerate;

    public override void Notify(Subject subject)
    {
        if (craftingBuilding == null)
            return;

        sliderProcess.value = craftingBuilding.craftingSlider.value;
    }

    public void SetData(CraftingBuilding craftingBuilding)
    {
        this.craftingBuilding = craftingBuilding;
        sliderProcess.maxValue = this.craftingBuilding.craftingSlider.maxValue;
        sliderProcess.value = this.craftingBuilding.craftingSlider.value;

        RefreshCurrentSlot();
        RefreshWaitingList();
    }

    public async void RefreshCurrentSlot()
    {
        if (craftingBuilding.CurrentRecipeStat != null)
        {
            sliderProcess.maxValue = this.craftingBuilding.CurrentRecipeStat.Workload;

            buttonAccelerate.interactable = true;
            imageCurrentSlot.sprite = await craftingBuilding.CurrentRecipeStat.RecipeData.GetProduct().GetImage();
            textCurrentSlot.text = craftingBuilding.CurrentRecipeStat.RecipeData.GetName();
        }
        else
        {
            sliderProcess.value = 0;
            buttonAccelerate.interactable = false;
            imageCurrentSlot.sprite = Addressables.LoadAssetAsync<Sprite>("Plane_Square_Round_3").WaitForCompletion();
            textCurrentSlot.text = "ÀÌ¸§";
        }
    }

    public async void RefreshWaitingList()
    {
        if (craftingBuilding.recipeStatList.Count > 0)
        {
            Queue<RecipeStat> copyList = new Queue<RecipeStat>(craftingBuilding.recipeStatList);
            for (int i = 0; i < imagesWaiting.Count; i++)
            {
                if (copyList.Count > 0)
                {
                    imagesWaiting[i].sprite = await copyList.Dequeue().RecipeData.GetProduct().GetImage();
                }
                else
                {
                    imagesWaiting[i].sprite = Addressables.LoadAssetAsync<Sprite>("Transparency").WaitForCompletion();
                }
            }
        }
        else
        {
            for (int i = 0; i < imagesWaiting.Count; i++)
            {
                imagesWaiting[i].sprite = Addressables.LoadAssetAsync<Sprite>("Plane_Square_Round_3").WaitForCompletion();
            }
        }
    }

    public void RefreshAll()
    {
        RefreshCurrentSlot();
        RefreshWaitingList();
    }

    public void OnClickAccelerate()
    {
        craftingBuilding.accumWorkLoad += 5000;
        sliderProcess.value = craftingBuilding.craftingSlider.value;

        if(sliderProcess.value >= sliderProcess.maxValue)
        {
            craftingBuilding.FinishCrafting();
            craftingBuilding.accumWorkLoad = BigNumber.Zero;
            sliderProcess.value = 0;
            RefreshCurrentSlot();
            RefreshWaitingList();
        }
    }
}