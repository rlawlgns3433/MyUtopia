using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiRecipeSlot : MonoBehaviour
{
    private static readonly string[] format =
    {
        "{0} : {1}",
        "{0} : {1} / {2} : {3}",
        "{0} : {1} / {2} : {3} / {4} : {5}"
    };

    public Image imagePortrait;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textRequireCurrency;
    public TextMeshProUGUI textRequireWorkload;
    public TextMeshProUGUI textSaleCoin;
    public TextMeshProUGUI textAmount;
    public Button buttonDecreaseAmount;
    public Button buttonIncreaseAmount;
    public Button buttonAutoCraft;
    public Button buttonCraft;

    public RecipeStat recipeStat;

    private void OnDestroy()
    {
        Destroy(imagePortrait);
        Destroy(textName);
        Destroy(textRequireCurrency);
        Destroy(textRequireWorkload);
        Destroy(textSaleCoin);
        Destroy(textAmount);
        Destroy(buttonDecreaseAmount);
        Destroy(buttonIncreaseAmount);
        Destroy(buttonAutoCraft);
        Destroy(buttonCraft);

        Destroy(gameObject);
    }

    public virtual void SetData(RecipeStat recipeStat) // virtual ���� ����
    {
        if (recipeStat == null)
            return;
        this.recipeStat = recipeStat;

        //imagePortrait.sprite = DataTableMgr.GetItemTable().Get(recipeStat.Product_ID).GetImage(); // ���� �̹��� ����
        textName.text = recipeStat.RecipeData.GetName();

        int count = -1;

        if (recipeStat.RecipeData.Resource_1 != 0)
            count++;
        if (recipeStat.RecipeData.Resource_2 != 0)
            count++;
        if (recipeStat.RecipeData.Resource_3 != 0)
            count++;

        switch(count)
        {
            case 0:
                    textRequireCurrency.text = string.Format(format[count], ((CurrencyType)recipeStat.RecipeData.Resource_1).ToString(), recipeStat.Resource_1_Value);
                break;
            case 1:
                textRequireCurrency.text = string.Format(format[count], ((CurrencyType)recipeStat.RecipeData.Resource_1).ToString(), recipeStat.Resource_1_Value,
                    ((CurrencyType)recipeStat.RecipeData.Resource_2).ToString(), recipeStat.Resource_2_Value);
                break;
            case 2:
                    textRequireCurrency.text = string.Format(format[count], ((CurrencyType)recipeStat.RecipeData.Resource_1).ToString(), recipeStat.Resource_1_Value,
                    ((CurrencyType)recipeStat.RecipeData.Resource_2).ToString(), recipeStat.Resource_2_Value, ((CurrencyType)recipeStat.RecipeData.Resource_3).ToString(), recipeStat.Resource_3_Value);
                break;
        }

        textRequireWorkload.text = recipeStat.RecipeData.Workload.ToString();
        textSaleCoin.text = recipeStat.RecipeData.GetProduct().Sell_Price;
    }

    public void OnCraftButtonClicked()
    {
        var storageProduct = FloorManager.Instance.floors["B3"].storage as StorageProduct;

        if(storageProduct.Count >= UiManager.Instance.productsUi.capacity)
        {
            Debug.Log("â�� ���� á���ϴ�.");
            return;
        }

        if(UiManager.Instance.craftTableUi.craftingBuilding.isCrafting)
        {
            Debug.Log("�������Դϴ�.");
            return;
        }

        /*
         1. ������ UI�� ���� & �߰� o
         2. ������ UI�� ������ ���
         3. ���� ���� ������Ʈ ����
         */
        // ��ȭ�� ����ϴٸ� ����
        if (recipeStat.Resource_1 != 0)
        {
            if (recipeStat.Resource_1_Value.ToBigNumber() > CurrencyManager.currency[(CurrencyType)recipeStat.Resource_1])
                return;
        }

        if (recipeStat.Resource_2 != 0)
        {
            if (recipeStat.Resource_2_Value.ToBigNumber() > CurrencyManager.currency[(CurrencyType)recipeStat.Resource_2])
                return;
        }

        if (recipeStat.Resource_3 != 0)
        {
            if (recipeStat.Resource_3_Value.ToBigNumber() > CurrencyManager.currency[(CurrencyType)recipeStat.Resource_3])
                return;
        }

        if (recipeStat.Resource_1 != 0)
        {
            CurrencyManager.currency[(CurrencyType)recipeStat.Resource_1] -= recipeStat.Resource_1_Value.ToBigNumber();
        }

        if (recipeStat.Resource_2 != 0)
        {
            CurrencyManager.currency[(CurrencyType)recipeStat.Resource_2] -= recipeStat.Resource_2_Value.ToBigNumber();
        }

        if (recipeStat.Resource_3 != 0)
        {
            CurrencyManager.currency[(CurrencyType)recipeStat.Resource_3] -= recipeStat.Resource_3_Value.ToBigNumber();
        }

        var uiCraftingTable = UiManager.Instance.craftTableUi;
        uiCraftingTable.uiCraftingList.Add(recipeStat);
        // ��� ���� â�� �ֱ�
        uiCraftingTable.craftingBuilding.Set(recipeStat);
        Destroy(gameObject);
    }
}
