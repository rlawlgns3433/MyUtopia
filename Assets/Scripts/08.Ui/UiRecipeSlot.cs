using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiRecipeSlot : MonoBehaviour
{
    public RecipeStat recipeStat;
    public Image imageProfile;
    public Button buttonCrafting;
    public TextMeshProUGUI textSalePrice;
    public TextMeshProUGUI textName;
    public RequireCurrencyProducts requireCurrencyProducts;
    public async void SetData(RecipeStat recipeStat)
    {
        this.recipeStat = recipeStat;
        imageProfile.sprite = await DataTableMgr.GetItemTable().Get(recipeStat.Product_ID).GetImage();
        textSalePrice.text = string.Format(StringTextFormatKr.formatSalePrice,
            DataTableMgr.GetItemTable().Get(recipeStat.Product_ID).Sell_Price.ToBigNumber().ToString());
        textName.text = recipeStat.RecipeData.GetName();
        requireCurrencyProducts.SetData(recipeStat);
    }

    public bool CheckCurrencyProduct()
    {
        foreach(var resource in recipeStat.Resources)
        {
            if (CurrencyManager.product[(CurrencyProductType)resource.Key] < resource.Value)
                return false;
        }
        return true;
    }

    public bool CheckFullList()
    {
        if (UiManager.Instance.craftTableUi.craftingBuilding.recipeStatList.Count >= 4)
            return true;

        return false;
    }

    public bool CheckFullStorage()
    {
        var storageProduct = FloorManager.Instance.GetFloor("B3").storage as StorageProduct;

        if (storageProduct.IsFull)
        {
            return true;
        }

        return false;
    }

    public void UseCurrencyProduct()
    {
        foreach (var resource in recipeStat.Resources)
        {
            if (CurrencyManager.product[(CurrencyProductType)resource.Key] >= resource.Value)
                CurrencyManager.product[(CurrencyProductType)resource.Key] -= resource.Value;
        }
    }

    public void OnClickCrafting()
    {
        if (CheckFullStorage())
        {
            UiManager.Instance.ShowWarningPanelUi();
            UiManager.Instance.warningPanelUi.SetWaring(WaringType.FullStorage);
            SoundManager.Instance.OnClickButton(SoundType.Caution);
            return;
        }

        if (CheckFullList())
        {
            UiManager.Instance.warningPanelUi.SetWaring(WaringType.FullList);
            UiManager.Instance.ShowWarningPanelUi();
            SoundManager.Instance.OnClickButton(SoundType.Caution);
            return;
        }

        if (CheckCurrencyProduct())
        {
            UseCurrencyProduct();
            UiManager.Instance.craftTableUi.craftingBuilding.Set(recipeStat);
            UiManager.Instance.craftTableUi.uiCraftingSlot.SetData(UiManager.Instance.craftTableUi.craftingBuilding);
            if (FloorManager.Instance.touchManager.tutorial != null)
            {
                if (FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.CreateItem)
                {
                    FloorManager.Instance.touchManager.tutorial.SetTutorialProgress();//Æ©Åä¸®¾ó
                }
            }
        }
    }
}