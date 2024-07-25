using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Furniture : MonoBehaviour, IClickable
{
    [SerializeField]
    private int facilityId;

    private FurnitureStat furnitureStat;
    public FurnitureStat FurnitureStat
    {
        get
        {
            if (furnitureStat == null)
                furnitureStat = new FurnitureStat(facilityId);

            return furnitureStat;
        }
        set
        {
            furnitureStat = value;
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

    public void LevelUp()
    {
        if (FurnitureStat.Level == FurnitureStat.Level_Max)
            return;

        if (CurrencyManager.currency[CurrencyType.Coin] < FurnitureStat.Level_Up_Coin_Value.ToBigNumber())
            return;

        FurnitureStat = new FurnitureStat(FurnitureStat.Furniture_ID + 1);
    }

    public void RegisterClickable()
    {
        ClickableManager.AddClickable(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        IsClicked = true;
    }
}
