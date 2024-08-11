using UnityEngine;
using UnityEngine.EventSystems;

public class ConductBuilding : Building
{
    private BigNumber touchProduce;
    protected override void OnEnable()
    {
        base.OnEnable();
        clickEvent += RefreshCurrency;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        DisplayFloatingText(touchProduce);
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

                var touchProduce = new BigNumber(BuildingStat.Touch_Produce);
                CurrencyManager.currency[buildingType] += touchProduce;
                this.touchProduce = touchProduce; // 플로팅 텍스트용
                break;
            case CurrencyType.CopperIngot:
            case CurrencyType.SilverIngot:
            case CurrencyType.GoldIngot:
                if (BuildingStat.IsLock)
                    return;
                if (CurrencyManager.currency[(CurrencyType)BuildingStat.Materials_Type] > BuildingStat.Conversion_rate)
                {
                    CurrencyManager.currency[buildingType] += 1;
                    this.touchProduce = new BigNumber(1);
                    CurrencyManager.currency[(CurrencyType)BuildingStat.Materials_Type] -= BuildingStat.Conversion_rate;
                }
                break;
        }
        MissionManager.Instance.AddMissionCountTargetId(buildingId);
        Debug.Log($"missionCount =>>{MissionManager.Instance.GetMissionCount(buildingId)}");
    }

    private void DisplayFloatingText(BigNumber bigNumber)
    {
        var pos = transform.position;
        pos.y += 1;

        DynamicTextManager.CreateText(pos, bigNumber.ToString(), DynamicTextManager.clickData, 2, 0.5f);
    }
}
