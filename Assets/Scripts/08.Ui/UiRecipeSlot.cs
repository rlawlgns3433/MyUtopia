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

    private int amount = 1;

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
    private StringTable stringTable;
    public StringTable StringTable
    {
        get
        {
            if(stringTable == null)
                stringTable = DataTableMgr.GetStringTable();

            return stringTable;
        }

        set
        {
            stringTable = value;
        }
    }

    private ResourceTable resourceTable;
    public ResourceTable ResourceTable
    {
        get
        {
            if (resourceTable == null)
                resourceTable = DataTableMgr.GetResourceTable();

            return resourceTable;
        }

        set
        {
            resourceTable = value;
        }
    }

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
                    textRequireCurrency.text = string.Format(format[count],
                        StringTable.Get(ResourceTable.Get(recipeStat.RecipeData.Resource_1).Resource_Name_ID), 
                        recipeStat.Resource_1_Value);
                break;
            case 1:
                    textRequireCurrency.text = string.Format(format[count],
                        StringTable.Get(ResourceTable.Get(recipeStat.RecipeData.Resource_1).Resource_Name_ID), recipeStat.Resource_1_Value,
                        StringTable.Get(ResourceTable.Get(recipeStat.RecipeData.Resource_2).Resource_Name_ID), recipeStat.Resource_2_Value);
                break;
            case 2:
                    textRequireCurrency.text = string.Format(format[count],
                        StringTable.Get(ResourceTable.Get(recipeStat.RecipeData.Resource_1).Resource_Name_ID), recipeStat.Resource_1_Value,
                        StringTable.Get(ResourceTable.Get(recipeStat.RecipeData.Resource_2).Resource_Name_ID), recipeStat.Resource_2_Value,
                        StringTable.Get(ResourceTable.Get(recipeStat.RecipeData.Resource_3).Resource_Name_ID), recipeStat.Resource_1_Value);
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
        if (!CheckResource(amount))
            return;

        if (recipeStat.Resource_1 != 0)
        {
            CurrencyManager.currency[(CurrencyType)recipeStat.Resource_1] -= recipeStat.Resource_1_Value.ToBigNumber() * amount;
        }

        if (recipeStat.Resource_2 != 0)
        {
            CurrencyManager.currency[(CurrencyType)recipeStat.Resource_2] -= recipeStat.Resource_2_Value.ToBigNumber() * amount;
        }

        if (recipeStat.Resource_3 != 0)
        {
            CurrencyManager.currency[(CurrencyType)recipeStat.Resource_3] -= recipeStat.Resource_3_Value.ToBigNumber() * amount;
        }

        var uiCraftingTable = UiManager.Instance.craftTableUi;
        uiCraftingTable.uiCraftingList.Add(recipeStat);
        uiCraftingTable.craftingBuilding.Set(recipeStat, amount);
        Destroy(gameObject);
    }

    public void OnClickIncreaseAmount()
    {

        if (!CheckResource(amount + 1))
            return; // ��ȭ ����

        amount += 1;
        textAmount.text = amount.ToString();
    }

    public void OnClickDecreaseAmount()
    {
        if(amount <= 1)
            return; // �ּ� ���� üũ
        
        amount -= 1;
        textAmount.text = amount.ToString();
    }

    public void OnClickAutoCraft()
    {

    }

    public bool CheckResource(int amount)
    {
        if (recipeStat.Resource_1 != 0)
        {
            if (recipeStat.Resource_1_Value.ToBigNumber() * amount > CurrencyManager.currency[(CurrencyType)recipeStat.Resource_1])
                return false;
        }

        if (recipeStat.Resource_2 != 0)
        {
            if (recipeStat.Resource_2_Value.ToBigNumber() * amount > CurrencyManager.currency[(CurrencyType)recipeStat.Resource_2])
                return false;
        }

        if (recipeStat.Resource_3 != 0)
        {
            if (recipeStat.Resource_3_Value.ToBigNumber() * amount > CurrencyManager.currency[(CurrencyType)recipeStat.Resource_3])
                return false;
        }

        return true;
    }
}
