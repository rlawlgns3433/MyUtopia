using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Building : MonoBehaviour, IClickable, IPointerClickHandler
{
    [SerializeField]
    private float duration = 0f;
    public BigNumber accumWorkLoad;
    public Vector3 initialScale;
    public Vector3 clickedScale;
    public CurrencyType buildingType;
    public int buildingId;
    public bool isLock = true;

    private BuildingData buildingData;
    public BuildingData BuildingData
    {
        get
        {
            if(buildingData.ID == 0)
                buildingData = DataTableMgr.Get<BuildingTable>(DataTableIds.Building).Get(buildingId);
            return buildingData;
        }
        set
        {
            buildingData = value;
        }
    }

    public event Action clickEvent;

    [SerializeField]
    private bool isClicked;
    public bool IsClicked 
    { 
        get
        {
            return isClicked;
        }
        
        set
        {
            isClicked = value;
            if (isClicked)
            {
                clickEvent?.Invoke();
                ClickableManager.OnClicked(this);
            }
        }
    }

    private void OnEnable()
    {
        RegisterClickable();
        clickEvent += RefreshCurrency;
    }

    private void Start()
    {
        transform.localScale = initialScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        IsClicked = true;

        transform.DOScale(clickedScale, duration).OnComplete(() =>
        {
            transform.DOScale(initialScale, duration);
        });

    }

    public void RegisterClickable()
    {
        ClickableManager.AddClickable(this);
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
                CurrencyManager.currency[(int)buildingType] += new BigNumber(BuildingData.Touch_Produce);
                break;
            case CurrencyType.CopperIngot:
            case CurrencyType.SilverIngot:
            case CurrencyType.GoldIngot:
                if (isLock)
                    return;
                if (CurrencyManager.currency[BuildingData.Materials_Type] > BuildingData.Conversion_rate)
                {
                    CurrencyManager.currency[(int)buildingType] += 1;
                    CurrencyManager.currency[BuildingData.Materials_Type] -= BuildingData.Conversion_rate;
                }
                break;
            case CurrencyType.Craft:
                accumWorkLoad += new BigNumber(BuildingData.Touch_Produce);
                Debug.Log(accumWorkLoad);
                break;
        }
    }
}