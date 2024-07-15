using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public class StorageData
{
    private string currWorkLoad;
    public BigNumber CurrentWorkLoad
    {
        get
        {
            return new BigNumber(currWorkLoad);
        }
        set
        {
            currWorkLoad = value.ToSimpleString();
        }
    }
    private string[] currArray;
    public BigNumber[] CurrArray
    {
        get
        {
            return currArray.Select(s => new BigNumber(s)).ToArray();
        }
        set
        {
            currArray = value.Select(bn => bn.ToSimpleString()).ToArray();
        }
    }
    private int totalOfflineTime;
    public int TotalOfflineTime
    {
        get
        {
            return totalOfflineTime;
        }
        set
        {
            totalOfflineTime = value;
        }
    }
}

public class Storage : MonoBehaviour, IClickable
{
    [SerializeField]
    private int facilityId;

    private FacilityStat facilityStat;
    public FacilityStat FacilityStat
    {
        get
        {
            if(facilityStat == null)
                facilityStat = new FacilityStat(facilityId);

            return facilityStat;
        }
        set
        {
            facilityStat = value;
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

    private void Awake()
    {
        clickEvent+= OpenProductStorage;
    }

    public void RegisterClickable()
    {
        ClickableManager.AddClickable(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        IsClicked = true;
    }

    public void OpenProductStorage()
    {
        UiManager.Instance.SetProductCapacity(FacilityStat.Effect_Value);
        UiManager.Instance.ShowProductsUi();
    }
}
