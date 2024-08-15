using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Threading;

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
    public Button buttonCraft; // 제작 버튼의 의미는 대기큐에 아이템을 거는 것

    public RecipeStat recipeStat;
    private StringTable stringTable;
    public StringTable StringTable
    {
        get
        {
            if (stringTable == null)
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
    
    public async virtual void SetData(RecipeStat recipeStat) // virtual 제거 예정
    {
        if (recipeStat == null)
            return;
        this.recipeStat = recipeStat;

        imagePortrait.sprite = await DataTableMgr.GetItemTable().Get(recipeStat.Product_ID).GetImage(); // 현재 이미지 없음
        textName.text = recipeStat.RecipeData.GetName();

        int count = -1;

        if (recipeStat.RecipeData.Resource_1 != 0)
            count++;
        if (recipeStat.RecipeData.Resource_2 != 0)
            count++;
        if (recipeStat.RecipeData.Resource_3 != 0)
            count++;

        switch (count)
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

        if (storageProduct.IsFull)
        {
            Debug.Log("창고가 가득 찼습니다.");
            return;
        }

        if(storageProduct.Count + UiManager.Instance.craftTableUi.craftingBuilding.recipeStatList.Count + 1 >= storageProduct.BuildingStat.Effect_Value)
        {
            Debug.Log("리스트가 가득 찼습니다.");
            return;
        }

        /*
         1. 제작중 UI를 생성 & 추가 o
         2. 제작중 UI에 정보를 출력
         3. 현재 게임 오브젝트 삭제
         */
        // 재화가 충분하다면 실행
        if (!CheckResource())
            return;

        if (recipeStat.Resource_1 != 0)
        {
            CurrencyManager.product[(CurrencyProductType)recipeStat.Resource_1] -= recipeStat.Resource_1_Value.ToBigNumber();
        }

        if (recipeStat.Resource_2 != 0)
        {
            CurrencyManager.product[(CurrencyProductType)recipeStat.Resource_2] -= recipeStat.Resource_2_Value.ToBigNumber();
        }

        if (recipeStat.Resource_3 != 0)
        {
            CurrencyManager.product[(CurrencyProductType)recipeStat.Resource_3] -= recipeStat.Resource_3_Value.ToBigNumber();
        }

        var uiCraftingTable = UiManager.Instance.craftTableUi;

        if(uiCraftingTable.craftingBuilding.isCrafting)
        {
            uiCraftingTable.uiCraftingSlot.SetWaitingList(recipeStat);
        }
        else
        {
            uiCraftingTable.uiCraftingSlot.SetData(recipeStat);
        }
        uiCraftingTable.craftingBuilding.Set(recipeStat);
        if(FloorManager.Instance.touchManager.tutorial != null)
        {
            FloorManager.Instance.touchManager.tutorial.SetTutorialProgress();
        }
    }


    public bool CheckResource(int amount = 1)
    {
        var storageProduct = FloorManager.Instance.floors["B3"].storage as StorageProduct;

        if (storageProduct.IsFull)
            return false;

        if(storageProduct.BuildingStat.Effect_Value - storageProduct.Count < amount)
        {
            return false;
        }

        if (recipeStat.Resource_1 != 0)
        {
            if (recipeStat.Resource_1_Value.ToBigNumber() * amount > CurrencyManager.product[(CurrencyProductType)recipeStat.Resource_1])
                return false;
        }

        if (recipeStat.Resource_2 != 0)
        {
            if (recipeStat.Resource_2_Value.ToBigNumber() * amount > CurrencyManager.product[(CurrencyProductType)recipeStat.Resource_2])
                return false;
        }

        if (recipeStat.Resource_3 != 0)
        {
            if (recipeStat.Resource_3_Value.ToBigNumber() * amount > CurrencyManager.product[(CurrencyProductType)recipeStat.Resource_3])
                return false;
        }

        return true;
    }
}
