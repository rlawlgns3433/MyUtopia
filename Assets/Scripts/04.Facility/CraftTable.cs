using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftTable : MonoBehaviour, IClickable
{
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

    public event Action clickEvent;

    private void Awake()
    {
        clickEvent += UiManager.Instance.ShowCraftTableUi;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isClicked = true;
    }

    public void RegisterClickable()
    {
        ClickableManager.AddClickable(this);
    }
}
