using UnityEngine;
using UnityEngine.UI;

public class UiWaitingSlot : MonoBehaviour
{
    public RecipeStat recipeStat;
    public Image imageWaiting;

    public async void SetData(RecipeStat recipeStat)
    {
        this.recipeStat = recipeStat;
        imageWaiting.sprite = await recipeStat.RecipeData.GetProduct().GetImage();
    }

    public void ClearData()
    {
        this.recipeStat = null;
        imageWaiting.sprite = null;
    }
}
