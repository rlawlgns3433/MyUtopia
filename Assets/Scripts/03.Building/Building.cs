using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Building : Subject, IClickable, IPointerClickHandler
{
    [SerializeField]
    private float duration = 0f;
    public VeinTest test;

    public Vector3 initialScale;
    public Vector3 clickedScale;
    public CurrencyType buildingType;

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
        Attach(test);
        RegisterClickable();
        clickEvent += OnClickBuilding;
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

        NotifyObservers();
    }

    public void RegisterClickable()
    {
        ClickableManager.AddClickable(this);
    }

    private void OnClickBuilding()
    {
        // 재화 저장 스크립트 필요
        // 각 재화를 얻는 로직을 작성

        switch (buildingType)
        {
            case CurrencyType.Coin:
                //CurrencyManager.currency[(int)CurrencyType.Coin] = 클릭당 획득 코인;
                break;
            case CurrencyType.CopperStone:
                //CurrencyManager.currency[(int)CurrencyType.CopperStone] = 클릭당 획득 코인;
                break;
            case CurrencyType.SilverStone:
                break;
            case CurrencyType.GoldStone:
                break;
            case CurrencyType.CopperIngot:
                break;
            case CurrencyType.SilverIngot:
                break;
            case CurrencyType.GoldIngot:
                break;
            default:
                break;
        }
    }
}