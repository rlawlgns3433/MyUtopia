using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Building : MonoBehaviour, IClickable, IPointerClickHandler, IGrowable
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
            if(buildingData.Building_ID == 0)
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

    protected virtual void OnEnable()
    {
        RegisterClickable();
    }

    protected virtual void Start()
    {
        transform.localScale = initialScale;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
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

    public void LevelUp()
    {
        if(BuildingData.Level == BuildingData.Level_Max)
            return;

        if (CurrencyManager.currency[CurrencyType.Coin] < BuildingData.Level_Up_Coin_Value.ToBigNumber())
            return;

        if (BuildingData.Level_Up_Resource_1 != 0)
        {
            if (BuildingData.Resource_1_Value.ToBigNumber() > CurrencyManager.currency[(CurrencyType)BuildingData.Level_Up_Resource_1])
                return;
        }

        if (BuildingData.Level_Up_Resource_2 != 0)
        {
            if (BuildingData.Resource_2_Value.ToBigNumber() > CurrencyManager.currency[(CurrencyType)BuildingData.Level_Up_Resource_2])
                return;
        }

        if (BuildingData.Level_Up_Resource_3 != 0)
        {
            if (BuildingData.Resource_3_Value.ToBigNumber() > CurrencyManager.currency[(CurrencyType)BuildingData.Level_Up_Resource_3])
                return;
        }

        CurrencyManager.currency[CurrencyType.Coin] -= BuildingData.Level_Up_Coin_Value.ToBigNumber();

        if (BuildingData.Level_Up_Resource_1 != 0)
        {
            CurrencyManager.currency[(CurrencyType)BuildingData.Level_Up_Resource_1] -= BuildingData.Resource_1_Value.ToBigNumber();
        }

        if (BuildingData.Level_Up_Resource_2 != 0)
        {
            CurrencyManager.currency[(CurrencyType)BuildingData.Level_Up_Resource_2] -= BuildingData.Resource_2_Value.ToBigNumber();
        }

        if (BuildingData.Level_Up_Resource_3 != 0)
        {
            CurrencyManager.currency[(CurrencyType)BuildingData.Level_Up_Resource_3] -= BuildingData.Resource_3_Value.ToBigNumber();
        }

        BuildingData = DataTableMgr.GetBuildingTable().Get(BuildingData.Building_ID + 100);
    }
}