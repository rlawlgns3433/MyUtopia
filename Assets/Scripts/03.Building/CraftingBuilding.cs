using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CraftingBuilding : Building
{
    public bool isCrafting = false;
    private RecipeStat currentRecipeStat;
    public RecipeStat CurrentRecipeStat
    {
        get
        {
            return currentRecipeStat;
        }
        set
        {
            currentRecipeStat = value;
        }
    }
    public Queue<RecipeStat> recipeStatList = new Queue<RecipeStat>();
    public Slider craftingSlider;

    protected override void Start()
    {
        base.Start();
        clickEvent += UiManager.Instance.ShowCraftTableUi;
    }

    private void Update()
    {
        if (isCrafting)
        {
            craftingSlider.value = accumWorkLoad.ToFloat();
        }
        else
        {
            craftingSlider.value = 0;
            craftingSlider.gameObject.SetActive(false);
        }
    }

    public void SetSlider()
    {
        craftingSlider.gameObject.SetActive(true);
        if(currentRecipeStat != null)
        {
            craftingSlider.maxValue = currentRecipeStat.Workload;
        }
        else
        {
            craftingSlider.maxValue = 1;
            craftingSlider.value = 0;
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
    }

    public void Set(RecipeStat recipeStat)
    {
        if (recipeStat == null)
            return;

        if(CurrentRecipeStat == null)
            CurrentRecipeStat = recipeStat;
        else
            recipeStatList.Enqueue(recipeStat);

        isCrafting = true;

        SetSlider();
    }

    public void FinishCrafting()
    {
        var storageProduct = (FloorManager.Instance.GetFloor("B3") as CraftingFloor).storage as StorageProduct;
        storageProduct.IncreaseProduct(CurrentRecipeStat.Product_ID);
        CurrentRecipeStat = null;
        isCrafting = false;

        if(recipeStatList.Count > 0)
        {
            Set(recipeStatList.Dequeue());
            SetSlider();
        }

        UiManager.Instance.craftTableUi.uiCraftingSlot.RefreshAll();
    }
}
