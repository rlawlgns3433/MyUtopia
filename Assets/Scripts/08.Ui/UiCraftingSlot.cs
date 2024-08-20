using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Collections.Generic;

public class UiCraftingSlot : Observer
{
    private static readonly string format = "{0} / {1}";
    public Image imageCurrentCrafting;
    public RecipeStat recipeCurrentCrafting;
    public List<UiWaitingSlot> waitingSlots = new List<UiWaitingSlot>();
    public TextMeshProUGUI textCurrentCraftingName;
    public TextMeshProUGUI textCurrentRemainProcess;
    public Slider sliderProcess;
    public Button buttonAccelerate;
    public bool CanHoldWaitingSlot
    {
        get
        {
            foreach(var slot in waitingSlots)
            {
                if(slot.recipeStat == null)
                {
                    return true;
                }
            }

            return false;
        }
    }

    private void OnEnable()
    {
        if (recipeCurrentCrafting == null)
        {
            buttonAccelerate.interactable = false;
            return;
        }
    }

    private void OnDestroy()
    {
        Destroy(imageCurrentCrafting.gameObject);
        Destroy(sliderProcess.gameObject);
        Destroy(gameObject);
    }

    public void SetWaitingList(RecipeStat recipeStat)
    {
        foreach (var slot in waitingSlots)
        {
            if (slot.recipeStat == null)
            {
                UiManager.Instance.craftTableUi.craftingBuilding.recipeStatList.Enqueue(recipeStat);
                slot.SetData(recipeStat);
                return;
            }
        }
    }

    public async virtual void SetData(RecipeStat recipeStat) // 레시피로 변경
    {
        if (recipeStat == null)
            return;

        recipeCurrentCrafting = recipeStat;
        sliderProcess.minValue = 0f;
        sliderProcess.maxValue = recipeCurrentCrafting.RecipeData.Workload;
        imageCurrentCrafting.sprite = await recipeCurrentCrafting.RecipeData.GetProduct().GetImage();
        textCurrentCraftingName.text = recipeCurrentCrafting.RecipeData.GetName();
        textCurrentRemainProcess.text = string.Format(format, sliderProcess.value.ToString(), recipeStat.RecipeData.Workload);
        buttonAccelerate.interactable = recipeCurrentCrafting != null ? true : false;

        var floor = FloorManager.Instance.GetFloor("B3");
        floor.AttachObserver(this);
    }

    public void OnClickAccelerate()
    {
        var building = UiManager.Instance.craftTableUi.craftingBuilding;

        if(recipeCurrentCrafting == null)
        {
            buttonAccelerate.interactable = false;
            return;
        }

        buttonAccelerate.interactable = true;

        UiManager.Instance.craftTableUi.craftingBuilding.accumWorkLoad += 5000; // 터치 업무량 추가 적용 필요
        sliderProcess.value = UiManager.Instance.craftTableUi.craftingBuilding.accumWorkLoad.ToFloat();
        textCurrentRemainProcess.text = sliderProcess.value.ToString();


        if (sliderProcess.value >= sliderProcess.maxValue)
        {
            (FloorManager.Instance.GetFloor("B3").storage as StorageProduct).IncreaseProduct(building.CurrentRecipeStat.Product_ID);

            if (building.recipeStatList.Count > 0)
            {
                building.CurrentRecipeStat = null;
                var nextRecipe = building.recipeStatList.Peek();
                building.Set(nextRecipe);
                UiManager.Instance.craftTableUi.RefreshAfterCrafting();
                building.CancelCrafting();
                recipeCurrentCrafting = nextRecipe;
            }
            else
            {
                building.CurrentRecipeStat = null;
                building.CancelCrafting();
                building.isCrafting = false; // 제작 끝
                
            }
            if(FloorManager.Instance.touchManager.tutorial != null)
            {
                if (FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.Accelerate)
                {
                    FloorManager.Instance.touchManager.tutorial.SetTutorialProgress();//튜토리얼
                }
            }
            sliderProcess.value = 0;
            building.SetSlider();
            textCurrentRemainProcess.text = building.craftingSlider.value.ToString();
        }
    }

    public override void Notify(Subject subject)
    {
        sliderProcess.value = UiManager.Instance.craftTableUi.craftingBuilding.accumWorkLoad.ToFloat();
        textCurrentRemainProcess.text = sliderProcess.value.ToString();
    }


    public void ReturnResources()
    {
        if (recipeCurrentCrafting.Resource_1 != 0)
        {
            CurrencyManager.currency[(CurrencyType)recipeCurrentCrafting.Resource_1] += recipeCurrentCrafting.Resource_1_Value.ToBigNumber();
        }

        if (recipeCurrentCrafting.Resource_2 != 0)
        {
            CurrencyManager.currency[(CurrencyType)recipeCurrentCrafting.Resource_2] += recipeCurrentCrafting.Resource_2_Value.ToBigNumber();
        }

        if (recipeCurrentCrafting.Resource_3 != 0)
        {
            CurrencyManager.currency[(CurrencyType)recipeCurrentCrafting.Resource_3] += recipeCurrentCrafting.Resource_3_Value.ToBigNumber();
        }
    }
}
