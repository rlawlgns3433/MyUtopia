using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Building : Subject, IClickable, IPointerClickHandler
{
    [SerializeField]
    private float duration = 0f;
    public System.Numerics.BigInteger accumWorkLoad;
    public Vector3 initialScale;
    public Vector3 clickedScale;
    public CurrencyType buildingType;
    public int buildingId;

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
                CurrencyManager.currency[(int)buildingType] += BuildingData.Touch_Produce.ToBigInteger();
                break;
            case CurrencyType.CopperIngot:

                if(CurrencyManager.currency[(int)CurrencyType.CopperStone] >= 1000) // 임시 비율
                {
                    CurrencyManager.currency[(int)CurrencyType.CopperIngot] += 1;
                    CurrencyManager.currency[(int)CurrencyType.CopperStone] -= 1000;
                }
                break;
            case CurrencyType.SilverIngot:
                if (CurrencyManager.currency[(int)CurrencyType.SilverStone] >= 1000) // 임시 비율
                {
                    CurrencyManager.currency[(int)CurrencyType.SilverIngot] += 1;
                    CurrencyManager.currency[(int)CurrencyType.SilverStone] -= 1000;
                }
                break;
            case CurrencyType.GoldIngot:
                if (CurrencyManager.currency[(int)CurrencyType.GoldStone] >= 1000) // 임시 비율
                {
                    CurrencyManager.currency[(int)CurrencyType.GoldIngot] += 1;
                    CurrencyManager.currency[(int)CurrencyType.GoldStone] -= 1000;
                }
                break;
            case CurrencyType.Craft:
                accumWorkLoad += BuildingData.Touch_Produce.ToBigInteger();
                Debug.Log(accumWorkLoad);
                break;
        }
    }
}