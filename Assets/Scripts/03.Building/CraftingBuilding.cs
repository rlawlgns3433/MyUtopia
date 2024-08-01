using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingBuilding : Building
{
    public bool isCrafting = false;
    public RecipeStat recipeStat;
    public Slider craftingSlider;
    public float craftingAccumLoad;
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
            craftingSlider.value = craftingSlider.maxValue - accumWorkLoad.ToFloat();

        }
    }

    public void SetSlider()
    {
        craftingSlider.gameObject.SetActive(true);
        craftingSlider.maxValue = recipeStat.Workload;
        craftingSlider.value = recipeStat.Workload;
    }

    public void CancelCrafting()
    {
        craftingSlider.gameObject.SetActive(false);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
    }

    public void Set(RecipeStat recipeStat)
    {
        if(recipeStat == null)
            return;
        this.recipeStat = recipeStat;
        isCrafting = true;
        SetSlider();
    }
}
