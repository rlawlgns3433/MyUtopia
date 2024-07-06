using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloorClick : MonoBehaviour, IClickable, IPointerClickHandler
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
                ClickableManager.OnClicked(this);
            }
        }
    }
    public event Action clickEvent;
    public CinemachineVirtualCamera vc;

    [SerializeField]
    private Vector3 focusOutPosition;
    [SerializeField]
    private Vector3 focusOutRotation;

    private void OnEnable()
    {
        RegisterClickable();
    }

    private void OnDisable()
    {
        ClickableManager.RemoveClickable(this);
        clickEvent -= FocusOut;
        clickEvent -= UnFollow;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        IsClicked = true;
    }

    public void RegisterClickable()
    {
        ClickableManager.AddClickable(this, UnFollow, FocusOut);
    }

    private void UnFollow()
    {
        vc.Follow = null;
        vc.LookAt = null;
    }

    private void FocusOut()
    {
        var transposer = vc.GetCinemachineComponent<CinemachineTransposer>();
        if (transposer != null)
        {
            transposer.m_FollowOffset = new Vector3(0, 3, -2);
            vc.transform.position = focusOutPosition;
            vc.transform.rotation = Quaternion.Euler(focusOutRotation);
        }
    }
}
