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

    public virtual void SetData(RecipeStat recipeStat) // virtual 제거 예정
    {
        if (recipeStat == null)
            return;
        this.recipeStat = recipeStat;

        //imagePortrait.sprite = DataTableMgr.GetItemTable().Get(recipeStat.Product_ID).GetImage(); // 현재 이미지 없음
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
            case 1:
                    textRequireCurrency.text = string.Format(format[count - 1], recipeStat.RecipeData.Resource_1, recipeStat.Resource_1_Value);
                break;
            case 2:
                textRequireCurrency.text = string.Format(format[count - 1], recipeStat.RecipeData.Resource_1, recipeStat.Resource_1_Value,
                    recipeStat.RecipeData.Resource_2, recipeStat.Resource_2_Value);
                break;
            case 3:
                    textRequireCurrency.text = string.Format(format[count - 1], recipeStat.RecipeData.Resource_1, recipeStat.Resource_1_Value,
                    recipeStat.RecipeData.Resource_2, recipeStat.Resource_2_Value, recipeStat.RecipeData.Resource_3, recipeStat.Resource_3_Value);
                break;
        }

        textRequireWorkload.text = recipeStat.RecipeData.Workload.ToString();
        textSaleCoin.text = recipeStat.RecipeData.GetProduct().Sell_Price;
    }

    public void OnCraftButtonClicked()
    {
        /*
         1. 제작중 UI를 생성 & 추가 o
         2. 제작중 UI에 정보를 출력
         3. 현재 게임 오브젝트 삭제
         */
        var uiCraftingTable = UiManager.Instance.craftTableUi;
        uiCraftingTable.uiCraftingList.Add(recipeStat);
        CurrencyManager.currency[CurrencyType.Craft] += 1;
        // 즉시 생산 창고에 넣기
        Destroy(gameObject);
    }
}
