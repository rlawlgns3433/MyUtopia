using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingBuilding : Building
{
    public bool isCrafting = false;
    public int amount = 1;
    public bool autoCrafting = false;
    public RecipeStat recipeStat;
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
        craftingSlider.maxValue = recipeStat.Workload;
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

    public void Set(RecipeStat recipeStat, bool autoCraft, int amount = 1)
    {
        if(recipeStat == null)
            return;

        this.autoCrafting = autoCraft;
        this.amount = amount;
        this.recipeStat = recipeStat;
        isCrafting = true;

        SetSlider();
    }

    public void UseResources()
    {
        if (recipeStat.Resource_1 != 0)
        {
            CurrencyManager.currency[(CurrencyType)recipeStat.Resource_1] -= recipeStat.Resource_1_Value.ToBigNumber();
        }

        if (recipeStat.Resource_2 != 0)
        {
            CurrencyManager.currency[(CurrencyType)recipeStat.Resource_2] -= recipeStat.Resource_2_Value.ToBigNumber();
        }

        if (recipeStat.Resource_3 != 0)
        {
            CurrencyManager.currency[(CurrencyType)recipeStat.Resource_3] -= recipeStat.Resource_3_Value.ToBigNumber();
        }
    }
}
