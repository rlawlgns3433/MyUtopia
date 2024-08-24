using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConductBuilding : Building
{
    private BigNumber touchProduce;

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        RefreshCurrency();
        DisplayFloatingText(touchProduce);
    }

    protected override void Start()
    {
        base.Start();
    }

    private void RefreshCurrency()
    {
        UniTask.WaitUntil(() => BuildingStat != null);

        if(BuildingStat.BuildingData == BuildingTable.defaultData)
        {
            BuildingStat = new BuildingStat(buildingId);
            Debug.Log("Null Building Stat");
        }

        switch (buildingType)
        {
            case CurrencyProductType.CopperStone:
            case CurrencyProductType.SilverStone:
            case CurrencyProductType.GoldStone:
                if (BuildingStat.IsLock)
                    return;
                if (BuildingStat.Touch_Produce == null)
                {
                    Debug.Log("Err TouchProduce");
                }
                var touchProduce = new BigNumber(BuildingStat.Touch_Produce);
         
                CurrencyManager.product[buildingType] += touchProduce;
                if (CurrencyManager.product == null)
                {
                    Debug.Log("Err Currency product");
                }

                this.touchProduce = touchProduce; // 플로팅 텍스트용
                MissionManager.Instance.AddMissionCountTargetId(buildingId);
                if (FloorManager.Instance.touchManager.tutorial != null)
                {
                    if (FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.TouchCopper)
                    {
                        FloorManager.Instance.touchManager.tutorial.tutorialTouchCount++;
                        if (FloorManager.Instance.touchManager.tutorial.tutorialTouchCount >= 10)
                        {
                            FloorManager.Instance.touchManager.tutorial.SetTutorialProgress();
                        }
                    }
                }
                break;
            case CurrencyProductType.CopperIngot:
            case CurrencyProductType.SilverIngot:
            case CurrencyProductType.GoldIngot:
                if (BuildingStat.IsLock)
                    return;
                if (CurrencyManager.product[(CurrencyProductType)BuildingStat.Materials_Type] > BuildingStat.Conversion_rate)
                {
                    CurrencyManager.product[buildingType] += 1;
                    CurrencyManager.product[(CurrencyProductType)BuildingStat.Materials_Type] -= BuildingStat.Conversion_rate;
                    this.touchProduce = new BigNumber(1);
                    if (CurrencyManager.product == null)
                    {
                        Debug.Log("Err TouchProduce");
                    }
                    MissionManager.Instance.AddMissionCountTargetId(buildingId);
                }
                else
                {
                    this.touchProduce = BigNumber.Zero;
                    if (CurrencyManager.product == null)
                    {
                        Debug.Log("Err TouchProduce");
                    }
                }

                if (FloorManager.Instance.touchManager == null)
                {
                    Debug.Log("Err touchManager");
                }

                if(FloorManager.Instance.touchManager.tutorial != null)
                {
                    if (FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.MakeIngot)
                    {
                        FloorManager.Instance.touchManager.tutorial.tutorialTouchCount++;
                        if (FloorManager.Instance.touchManager.tutorial.tutorialTouchCount >= 10)
                        {
                            FloorManager.Instance.touchManager.tutorial.SetTutorialProgress();
                        }
                    }
                }
                break;
        }
    }

    private void DisplayFloatingText(BigNumber bigNumber)
    {
        if (bigNumber <= BigNumber.Zero)
            return;

        var pos = transform.position;

        switch (buildingType)
        {
            case CurrencyProductType.CopperStone:
            case CurrencyProductType.SilverStone:
            case CurrencyProductType.GoldStone:
                pos.y += 1.5f;
                break;
            case CurrencyProductType.CopperIngot:
            case CurrencyProductType.SilverIngot:
            case CurrencyProductType.GoldIngot:
                pos.y += 3f;
                break;
        }

        DynamicTextManager.CreateText(pos, bigNumber.ToString(), DynamicTextManager.clickData, 2, 0.5f);
    }
}
