using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Vein : Subject, IPointerClickHandler
{
    [SerializeField]
    private float duration = 0f;
    public VeinTest test;

    public Vector3 initialScale;
    public Vector3 clickedScale;

    private void OnEnable()
    {
        Attach(test);
    }

    private void Start()
    {
        transform.localScale = initialScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.DOScale(clickedScale, duration).OnComplete(() =>
        {
            transform.DOScale(initialScale, duration);
        });

        NotifyObservers();
    }
}