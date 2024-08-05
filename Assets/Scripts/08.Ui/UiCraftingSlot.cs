using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UiCraftingSlot : Observer
{
    private static readonly string format = "{0} / {1}";
    private static readonly string formatRemainAmount = "{0}개";
    public Image imagePortrait;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textRemainAmount;
    public TextMeshProUGUI textRemainProcess;
    public Slider sliderProcess;
    public Button buttonCancel;
    public RecipeStat recipeStat;
    private int amount = 1;
    private bool autoCraft = false;

    private void OnDestroy()
    {
        Destroy(imagePortrait.gameObject);
        Destroy(sliderProcess.gameObject);
        Destroy(gameObject);
    }

    public virtual void SetData(RecipeStat recipeStat, bool autoCraft = false, int amount = 1) // 레시피로 변경
    {
        if (recipeStat == null)
            return;
        this.amount = amount;
        this.autoCraft = autoCraft;
        this.recipeStat = recipeStat;
        sliderProcess.minValue = 0f;
        sliderProcess.maxValue = recipeStat.RecipeData.Workload;

        textRemainAmount.text = string.Format(formatRemainAmount, amount <= 0 ? 1 : amount);
        textName.text = this.recipeStat.RecipeData.GetName();
        textRemainProcess.text = string.Format(format, sliderProcess.value.ToString(), recipeStat.RecipeData.Workload);
        var floor = FloorManager.Instance.GetFloor("B3");
        floor.AttachObserver(this);
    }

    public void OnClickCancel()
    {
        // 취소 재화 지급
        ReturnResources();

        amount = 0;
        UiManager.Instance.craftTableUi.craftingBuilding.autoCrafting = false;

        ReturnRecipe();
        UiManager.Instance.craftTableUi.craftingBuilding.SetSlider();
    }

    public override void Notify(Subject subject)
    {
        sliderProcess.value = UiManager.Instance.craftTableUi.craftingBuilding.accumWorkLoad.ToFloat();
        textRemainProcess.text = sliderProcess.value.ToString();
        if (sliderProcess.value == 0f)
        {
            //if (UiManager.Instance.craftTableUi.craftingBuilding.autoCrafting)
            //    return;
            if ((FloorManager.Instance.GetFloor("B3").storage as StorageProduct).IsFull)
            {
                if(amount-- >= 1)
                {
                    ReturnResources();
                }
                amount = 0;
            }

            amount--;
            textRemainAmount.text = string.Format(formatRemainAmount, amount <= 0 ? 1 : amount);

            ReturnRecipe();
        }
    }

    public void ReturnRecipe()
    {
        if (!UiManager.Instance.craftTableUi.craftingBuilding.autoCrafting)
        {
            if (amount >= 1)
                return;

            var uiCraftingTable = UiManager.Instance.craftTableUi;
            UiManager.Instance.craftTableUi.craftingBuilding.isCrafting = false;
            UiManager.Instance.craftTableUi.craftingBuilding.CancelCrafting();
            uiCraftingTable.uiRecipeList.Add(recipeStat, UiManager.Instance.craftTableUi.craftingBuilding.autoCrafting);
            Destroy(gameObject);
            return;
        }

        if (amount < 1)
        {
            if (UiManager.Instance.craftTableUi.craftingBuilding.autoCrafting)
                return;
            UiManager.Instance.craftTableUi.craftingBuilding.autoCrafting = false;
            var uiCraftingTable = UiManager.Instance.craftTableUi;
            uiCraftingTable.uiRecipeList.Add(recipeStat, UiManager.Instance.craftTableUi.craftingBuilding.autoCrafting);
            UiManager.Instance.craftTableUi.craftingBuilding.isCrafting = false;
            UiManager.Instance.craftTableUi.craftingBuilding.CancelCrafting();
            Destroy(gameObject);
            return;
        }
    }


    public void ReturnResources()
    {
        if (recipeStat.Resource_1 != 0)
        {
            CurrencyManager.currency[(CurrencyType)recipeStat.Resource_1] += recipeStat.Resource_1_Value.ToBigNumber() * (amount <= 0 ? 1 : amount);
        }

        if (recipeStat.Resource_2 != 0)
        {
            CurrencyManager.currency[(CurrencyType)recipeStat.Resource_2] += recipeStat.Resource_2_Value.ToBigNumber() * (amount <= 0 ? 1 : amount);
        }

        if (recipeStat.Resource_3 != 0)
        {
            CurrencyManager.currency[(CurrencyType)recipeStat.Resource_3] += recipeStat.Resource_3_Value.ToBigNumber() * (amount <= 0 ? 1 : amount);
        }
    }


}
