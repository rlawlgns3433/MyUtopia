using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiCraftingSlot : MonoBehaviour
{
    private static readonly string format = "{0} / {1}";
    public Image imagePortrait;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textRemainAmount;
    public TextMeshProUGUI textRemainProcess;
    public Slider sliderProcess;
    public Button buttonCancel;
    public RecipeStat recipeStat;

    private void OnDestroy()
    {
        Destroy(imagePortrait.gameObject);
        Destroy(sliderProcess.gameObject);
        Destroy(gameObject);
    }

    public virtual void SetData(RecipeStat recipeStat) // 레시피로 변경
    {
        if (recipeStat == null)
            return;
        this.recipeStat = recipeStat;
        sliderProcess.minValue = 0f;
        sliderProcess.maxValue = recipeStat.RecipeData.Workload;

        textName.text = this.recipeStat.RecipeData.GetName();
        textRemainAmount.text = "0"; // 임시 코드
        textRemainProcess.text = string.Format(sliderProcess.value.ToString(), recipeStat.RecipeData.Workload);
    }

    public void OnClickCancel()
    {
        var uiCraftingTable = UiManager.Instance.craftTableUi;
        uiCraftingTable.uiRecipeList.Add(recipeStat);
        Destroy(gameObject);
    }
}
