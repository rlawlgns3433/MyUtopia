using System;
using UnityEngine;
using UnityEngine.EventSystems;
public class StorageValue : MonoBehaviour, IClickable
{
    public GameObject storageUi;
    private int totalValue;
    public int TotalValue
    {
        get
        {
            return totalValue;
        }
        set
        {
            totalValue = value;
        }
    }

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
        clickEvent += OpenStorageUi;
        clickEvent += UiManager.Instance.SetSwipeDisable;
        RegisterClickable();
    }

    public void OpenStorageUi()
    {
        if(!storageUi.gameObject.activeSelf)
        {
            storageUi.gameObject.SetActive(true);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        IsClicked = true;
        Debug.Log("StorageClick");
    }

    public void RegisterClickable()
    {
        ClickableManager.AddClickable(this);
    }
}
