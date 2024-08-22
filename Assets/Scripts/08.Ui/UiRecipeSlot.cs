using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Runtime.Serialization;

public class UiRecipeSlot : MonoBehaviour
{
    private static readonly string format = "판매 가격 : {0}";

    public Image imagePortrait;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textRequireCurrency; // 리스트 형태로 변경 필요
    public TextMeshProUGUI textSaleCoin;
    public Button buttonCraft;
    public UiRequireCurrencyProduct uiRequireCurrencyProduct;
    public RequireCurrencyProducts requireCurrencyProducts;

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

        imagePortrait.sprite = await DataTableMgr.GetItemTable().Get(recipeStat.Product_ID).GetImage();
        textName.text = recipeStat.RecipeData.GetName();

        //int count = -1;

        requireCurrencyProducts.SetData(recipeStat);
        textSaleCoin.text = string.Format(format, new BigNumber(recipeStat.RecipeData.GetProduct().Sell_Price).ToString());
    }

    public void OnCraftButtonClicked()
    {
        var storageProduct = FloorManager.Instance.floors["B3"].storage as StorageProduct;

        if (storageProduct.IsFull)
        {
            Debug.Log("창고가 가득 찼습니다.");
            UiManager.Instance.warningPanelUi.SetWaring(WaringType.FullStorage);
            UiManager.Instance.ShowWarningPanelUi();
            return;
        }

        int count = storageProduct.Count + UiManager.Instance.craftTableUi.craftingBuilding.recipeStatList.Count;
        if (UiManager.Instance.craftTableUi.uiCraftingSlot.recipeCurrentCrafting != null)
            count++;

        if (count >= storageProduct.BuildingStat.Effect_Value)
        {
            Debug.Log("리스트가 가득 찼습니다.");
            UiManager.Instance.warningPanelUi.SetWaring(WaringType.FullList);
            UiManager.Instance.ShowWarningPanelUi();
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
