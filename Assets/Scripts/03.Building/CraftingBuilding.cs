using UnityEngine.EventSystems;

public class CraftingBuilding : Building
{
    public bool isCrafting = false;
    public RecipeStat recipeStat;

    protected override void OnEnable()
    {
        base.OnEnable();
        clickEvent += UiManager.Instance.ShowCraftTableUi;
    }

    protected override void Start()
    {
        base.Start();
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
    }
}
