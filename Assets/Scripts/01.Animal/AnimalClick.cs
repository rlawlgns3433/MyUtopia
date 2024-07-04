using Cinemachine;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimalClick : MonoBehaviour, IClickable
{
    [SerializeField]
    private bool isClicked = false;
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
                ClickableManager.AddClickable(this);
            }
        }
    }
    public event Action clickEvent;
    public CinemachineVirtualCamera vc;

    private void Awake()
    {
        clickEvent += Bump;
        clickEvent += Follow;
        RegisterClickable();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        IsClicked = true;
    }

    public void Follow()
    {
        vc.Follow = transform;
        vc.LookAt = transform;
        var transposer = vc.GetCinemachineComponent<CinemachineTransposer>();
        if (transposer != null)
        {
            transposer.m_FollowOffset = new Vector3(0,2,-2);
        }
    }

    private void Bump()
    {
        transform.DOScale(new Vector3(2f, 2f, 2f), 0.3f).OnComplete(() =>
        {
            transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f);
        });
    }

    public void RegisterClickable()
    {
        ClickableManager.AddClickable(this);
    }
}
