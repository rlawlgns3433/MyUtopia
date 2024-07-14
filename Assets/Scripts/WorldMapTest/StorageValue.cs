using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        RegisterClickable();
    }

    public void OpenStorageUi()
    {
        if (!storageUi.gameObject.activeSelf)
        {
            gameObject.SetActive(false );
            storageUi.gameObject.SetActive(true);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        IsClicked = true;
    }

    public void RegisterClickable()
    {
        ClickableManager.AddClickable(this);
    }
}
