using UnityEngine.EventSystems;

public class ConductBuilding : Building
{
    protected override void OnEnable()
    {
        base.OnEnable();
        clickEvent += RefreshCurrency;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
    }

    protected override void Start()
    {
        base.Start();
    }

    private void RefreshCurrency()
    {
        switch (buildingType)
        {
            case CurrencyType.Coin:
            case CurrencyType.CopperStone:
            case CurrencyType.SilverStone:
            case CurrencyType.GoldStone:
                if (isLock)
                    return;
                CurrencyManager.currency[buildingType] += new BigNumber(BuildingData.Touch_Produce);
                break;
            case CurrencyType.CopperIngot:
            case CurrencyType.SilverIngot:
            case CurrencyType.GoldIngot:
                if (isLock)
                    return;
                if (CurrencyManager.currency[(CurrencyType)BuildingData.Materials_Type] > BuildingData.Conversion_rate)
                {
                    CurrencyManager.currency[buildingType] += 1;
                    CurrencyManager.currency[(CurrencyType)BuildingData.Materials_Type] -= BuildingData.Conversion_rate;
                }
                break;
        }
    }
}
