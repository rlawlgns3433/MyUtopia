using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingBuilding : Building
{
    public bool isCrafting = false;
    public RecipeStat currentRecipeStat;
    public Queue<RecipeStat> recipeStatList = new Queue<RecipeStat>();
    public Slider craftingSlider;
    protected override void OnEnable()
    {
        base.OnEnable();
        clickEvent += UiManager.Instance.ShowCraftTableUi;
    }

    protected override void Start()
    {
        base.Start();
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
        craftingSlider.maxValue = currentRecipeStat.Workload;
        craftingSlider.value = craftingSlider.minValue;
    }

    public void CancelCrafting()
    {
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
            currentRecipeStat = recipeStat;

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
