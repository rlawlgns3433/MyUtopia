using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Building : MonoBehaviour, IClickable, IGrowable
{
    public float duration = 0f;
    public BigNumber accumWorkLoad;
    public Vector3 initialScale;
    public Vector3 clickedScale;
    public CurrencyProductType buildingType;
    public int buildingId;
    public bool IsUpgrading { get; set; }

    private BuildingStat buildingStat;
    public BuildingStat BuildingStat
    {
        get
        {
            if(buildingStat == null)
            {
                buildingStat = new BuildingStat(buildingId);
            }
            return buildingStat;
        }
        set
        {
            buildingStat = value;
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
                ClickableManager.OnClicked(this);
                clickEvent?.Invoke();
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
        if(BuildingStat.Level == BuildingStat.Level_Max)
            return;

        BuildingStat = new BuildingStat(BuildingStat.Building_ID + 100);
        BuildingStat.IsLock = false;
    }

    public bool CheckCurrency()
    {
        if (CurrencyManager.currency[CurrencyType.Coin] < BuildingStat.Level_Up_Coin_Value.ToBigNumber())
            return false;

        if (BuildingStat.Level_Up_Resource_1 != 0)
        {
            if (BuildingStat.Resource_1_Value.ToBigNumber() > CurrencyManager.currency[(CurrencyType)BuildingStat.Level_Up_Resource_1])
                return false;
        }

        if (BuildingStat.Level_Up_Resource_2 != 0)
        {
            if (BuildingStat.Resource_2_Value.ToBigNumber() > CurrencyManager.currency[(CurrencyType)BuildingStat.Level_Up_Resource_2])
                return false;
        }

        if (BuildingStat.Level_Up_Resource_3 != 0)
        {
            if (BuildingStat.Resource_3_Value.ToBigNumber() > CurrencyManager.currency[(CurrencyType)BuildingStat.Level_Up_Resource_3])
                return false;
        }

        return true;
    }

    public void SpendCurrency()
    {

        CurrencyManager.currency[CurrencyType.Coin] -= BuildingStat.Level_Up_Coin_Value.ToBigNumber();

        if (BuildingStat.Level_Up_Resource_1 != 0)
        {
            CurrencyManager.currency[(CurrencyType)BuildingStat.Level_Up_Resource_1] -= BuildingStat.Resource_1_Value.ToBigNumber();
        }

        if (BuildingStat.Level_Up_Resource_2 != 0)
        {
            CurrencyManager.currency[(CurrencyType)BuildingStat.Level_Up_Resource_2] -= BuildingStat.Resource_2_Value.ToBigNumber();
        }

        if (BuildingStat.Level_Up_Resource_3 != 0)
        {
            CurrencyManager.currency[(CurrencyType)BuildingStat.Level_Up_Resource_3] -= BuildingStat.Resource_3_Value.ToBigNumber();
        }
    }
}