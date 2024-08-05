using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiCraftingSlot : Observer
{
    private static readonly string format = "{0} / {1}";
    public Image imagePortrait;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textRemainAmount;
    public TextMeshProUGUI textRemainProcess;
    public Slider sliderProcess;
    public Button buttonCancel;
    public RecipeStat recipeStat;
    private int amount = 1;

    private void OnDestroy()
    {
        Destroy(imagePortrait.gameObject);
        Destroy(sliderProcess.gameObject);
        Destroy(gameObject);
    }

    public virtual void SetData(RecipeStat recipeStat, int amount = 1) // 레시피로 변경
    {
        if (recipeStat == null)
            return;
        this.amount = amount;

        this.recipeStat = recipeStat;
        sliderProcess.minValue = 0f;
        sliderProcess.maxValue = recipeStat.RecipeData.Workload;

        textName.text = this.recipeStat.RecipeData.GetName();
        textRemainAmount.text = "0"; // 임시 코드
        textRemainProcess.text = string.Format(format, sliderProcess.value.ToString(), recipeStat.RecipeData.Workload);
        var floor = FloorManager.Instance.GetFloor("B3");
        floor.AttachObserver(this);
    }

    public void OnClickCancel()
    {
        // 취소 재화 지급
        if (recipeStat.Resource_1 != 0)
        {
            CurrencyManager.currency[(CurrencyType)recipeStat.Resource_1] += recipeStat.Resource_1_Value.ToBigNumber() * amount;
        }

        if (recipeStat.Resource_2 != 0)
        {
            CurrencyManager.currency[(CurrencyType)recipeStat.Resource_2] += recipeStat.Resource_2_Value.ToBigNumber() * amount;
        }

        if (recipeStat.Resource_3 != 0)
        {
            CurrencyManager.currency[(CurrencyType)recipeStat.Resource_3] += recipeStat.Resource_3_Value.ToBigNumber() * amount;
        }

        amount = 0;
        ReturnRecipe();
        UiManager.Instance.craftTableUi.craftingBuilding.SetSlider();
    }

    public override void Notify(Subject subject)
    {
        sliderProcess.value = UiManager.Instance.craftTableUi.craftingBuilding.accumWorkLoad.ToFloat();
        textRemainProcess.text = sliderProcess.value.ToString();
        if (sliderProcess.value == 0f)
        {
            amount--;
            ReturnRecipe();
        }
    }

    public void ReturnRecipe()
    {
        if(amount < 1)
        {
            var uiCraftingTable = UiManager.Instance.craftTableUi;
            uiCraftingTable.uiRecipeList.Add(recipeStat);
            UiManager.Instance.craftTableUi.craftingBuilding.isCrafting = false;
            UiManager.Instance.craftTableUi.craftingBuilding.CancelCrafting();
            Destroy(gameObject);
        }
    }
}
