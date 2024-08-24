using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;
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
            if(currentRecipeStat == null)
                UiManager.Instance.craftTableUi.uiCraftingSlot.buttonAccelerate.interactable = false;
            else
                UiManager.Instance.craftTableUi.uiCraftingSlot.buttonAccelerate.interactable = true;
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
            craftingSlider.gameObject.SetActive(false);
        }
    }

    public void SetSlider()
    {
        craftingSlider.gameObject.SetActive(true);
        if(currentRecipeStat != null)
        {
            craftingSlider.maxValue = currentRecipeStat.Workload;
            craftingSlider.value = craftingSlider.minValue;
        }
        else
        {
            craftingSlider.maxValue = 1;
            craftingSlider.value = 0;
        }
    }

    public void CancelCrafting()
    {
        if(CurrentRecipeStat != null)
        {
            MissionManager.Instance.AddMissionCountMakeItem(CurrentRecipeStat.Product_ID);
            MissionManager.Instance.AddMissionCountMakeItem(0);
        }

        UiManager.Instance.craftTableUi.uiCraftingSlot.recipeCurrentCrafting = null;

        if (recipeStatList.Count <= 0 && currentRecipeStat == null)
        {
            AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>("Plane_Square_Round_3");
            handle.Completed += (AsyncOperationHandle<Sprite> obj) =>
            {
                UiManager.Instance.craftTableUi.uiCraftingSlot.imageCurrentCrafting.sprite = obj.Result;
            };

            UiManager.Instance.craftTableUi.uiCraftingSlot.textCurrentCraftingName.text = "¿Ã∏ß";
        }

        craftingSlider.gameObject.SetActive(false);
        accumWorkLoad = BigNumber.Zero;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
    }

    public void Set(RecipeStat recipeStat)
    {
        if (recipeStat == null)
            return;

        if(currentRecipeStat == null)
            CurrentRecipeStat = recipeStat;

        isCrafting = true;

        SetSlider();
    }

    public void UseResources()
    {
        if (currentRecipeStat.Resource_1 != 0)
        {
            CurrencyManager.currency[(CurrencyType)currentRecipeStat.Resource_1] -= currentRecipeStat.Resource_1_Value.ToBigNumber();
        }

        if (currentRecipeStat.Resource_2 != 0)
        {
            CurrencyManager.currency[(CurrencyType)currentRecipeStat.Resource_2] -= currentRecipeStat.Resource_2_Value.ToBigNumber();
        }

        if (currentRecipeStat.Resource_3 != 0)
        {
            CurrencyManager.currency[(CurrencyType)currentRecipeStat.Resource_3] -= currentRecipeStat.Resource_3_Value.ToBigNumber();
        }
    }
}
