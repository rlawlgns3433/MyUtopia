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
        // ��ȭ ���� ��ũ��Ʈ �ʿ�
        // �� ��ȭ�� ��� ������ �ۼ�

        switch (buildingType)
        {
            case CurrencyType.Coin:
                //CurrencyManager.currency[(int)CurrencyType.Coin] = Ŭ���� ȹ�� ����;
                break;
            case CurrencyType.CopperStone:
                //CurrencyManager.currency[(int)CurrencyType.CopperStone] = Ŭ���� ȹ�� ����;
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