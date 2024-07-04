using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Vein : Subject, IClickable, IPointerClickHandler
{
    [SerializeField]
    private float duration = 0f;
    public VeinTest test;

    public Vector3 initialScale;
    public Vector3 clickedScale;

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
}