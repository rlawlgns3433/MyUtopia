using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UiCraftingSlot : MonoBehaviour
{
    public Image imagePortrait;
    public Slider sliderProcess;
    public RecipeStat recipeStat;

    private void OnDestroy()
    {
        Destroy(imagePortrait.gameObject);
        Destroy(sliderProcess.gameObject);
        Destroy(gameObject);
    }

    public virtual void SetData(RecipeStat recipeStat) // 레시피로 변경
    {
        if (this.recipeStat == null)
            return;
        this.recipeStat = recipeStat;

        //if(animalData.GetProfile() != null)
        //    imagePortrait.sprite = animalData.GetProfile();

        sliderProcess.minValue = 0f;
        sliderProcess.maxValue = recipeStat.RecipeData.Workload;
        //sliderProcess.value = recipeStat.Stamina;
    }
}
