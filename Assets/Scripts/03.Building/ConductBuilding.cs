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
                if (BuildingStat.IsLock)
                    return;
                CurrencyManager.currency[buildingType] += new BigNumber(BuildingStat.Touch_Produce);
                break;
            case CurrencyType.CopperIngot:
            case CurrencyType.SilverIngot:
            case CurrencyType.GoldIngot:
                if (BuildingStat.IsLock)
                    return;
                if (CurrencyManager.currency[(CurrencyType)BuildingStat.Materials_Type] > BuildingStat.Conversion_rate)
                {
                    CurrencyManager.currency[buildingType] += 1;
                    CurrencyManager.currency[(CurrencyType)BuildingStat.Materials_Type] -= BuildingStat.Conversion_rate;
                }
                break;
        }
    }
}
