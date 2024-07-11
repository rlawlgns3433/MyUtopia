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
    private Slider currencyValue;
    private int offLineSeconds;
    private int maxValue = 100;
    private float normalizedValue;

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

    private async void Start()
    {
        await UniTask.WaitUntil(() => UtilityTime.Seconds > 0);
        offLineSeconds = UtilityTime.Seconds;
        currencyValue = GetComponent<Slider>();
        SetValue();
    }

    private void SetValue()
    {
        if (offLineSeconds > 0 && offLineSeconds < maxValue)
        {
            normalizedValue = Mathf.Clamp01((float)offLineSeconds / maxValue);
            currencyValue.value = normalizedValue;
        }
        else if(offLineSeconds >= maxValue)
        {
            normalizedValue = Mathf.Clamp01((float)offLineSeconds / maxValue);
            currencyValue.value = normalizedValue;
        }
        else if(offLineSeconds <= 0)
        {
            gameObject.SetActive(false);
        }
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
    }

    public void RegisterClickable()
    {
        ClickableManager.AddClickable(this);
    }
}
