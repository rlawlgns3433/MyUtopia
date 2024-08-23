using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class UiWaitingSlot : MonoBehaviour
{
    public RecipeStat recipeStat;
    public Sprite spriteNull;
    public Image imageWaiting;

    public async void SetData(RecipeStat recipeStat)
    {
        this.recipeStat = recipeStat;
        imageWaiting.sprite = await recipeStat.RecipeData.GetProduct().GetImage();
    }

    public void ClearData()
    {
        imageWaiting.sprite = Addressables.LoadAssetAsync<Sprite>("Transparency").WaitForCompletion();
        recipeStat = null;
    }
}
