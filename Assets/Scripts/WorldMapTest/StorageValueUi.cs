using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StorageValueUi : MonoBehaviour, IClickable
{
    public List<CurrencyType> currencyTypes;
    private BigNumber[] currentValue;
    private BigNumber[] currentWorkLoads;
    public GameObject[] currencyValues;
    private int maxValue;
    private int totalValue;
    private StorageTest storageTest;
    public TextMeshProUGUI timeDifferenceText;

    public event Action clickEvent;

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
    private void Awake()
    {
        clickEvent += Set;
        RegisterClickable();
    }

    private void OnEnable()
    {
        storageTest = GetComponentInParent<StorageTest>();
        currentValue = new BigNumber[storageTest.CurrArray.Length];
        currentWorkLoads = new BigNumber[storageTest.CurrArray.Length];
        maxValue = storageTest.MaxSeconds;
        totalValue = storageTest.CurrentTotalSeconds;
        var maxValueText = ConvertSecondsToHours(maxValue);
        var totalValueText = ConvertSecondsToHours(totalValue);
        timeDifferenceText.text = $"{totalValueText} / {maxValueText}";
        for(int i = 0; i < storageTest.CurrArray.Length; i++)
        {
            currentValue[i] = storageTest.CurrArray[i];
            currentWorkLoads[i] = storageTest.Values[i];
        }
        for (int i = 0; i < storageTest.CurrArray.Length; i++)
        {
            currencyValues[i].gameObject.SetActive(true);
            var currencyValueText = currencyValues[i].GetComponentInChildren<TextMeshProUGUI>();
            currencyValueText.text = currentValue[i].ToString();
            var currencyValueSlider = currencyValues[i].GetComponentInChildren<Slider>();
            var maxSeconds = maxValue / 3;
            if(currentValue[i] > 0)
            {
                //currencyValueSlider.value = Mathf.Clamp01((float)totalValue / maxValue);
                //Debug.Log(currencyValueSlider.value);
                currentWorkLoads[i] *= maxSeconds;
                var clampValue = BigNumber.ToFloatClamped01(currentValue[i], currentWorkLoads[i]);
                
                currencyValueSlider.value = clampValue;
                Debug.Log("test" + currencyValueSlider.value);
                //Debug.Log(("test" + clampValue.ToFloat()));
            }
            else
            {
                currencyValueSlider.value = 0;
            }
        }
    }

    public void ExitUi()
    {
        gameObject.SetActive(false);
    }
    public string ConvertSecondsToHours(int seconds)
    {
        if (seconds >= 3600)
        {
            double hours = seconds / 3600.0;
            double truncatedHours = System.Math.Truncate(hours * 10) / 10;
            return truncatedHours.ToString("0.#") + "h";
        }
        else if (seconds >= 60)
        {
            double minutes = seconds / 60.0;
            double truncatedMinutes = System.Math.Truncate(minutes * 10) / 10;
            return truncatedMinutes.ToString("0.#") + "m";
        }
        else
        {
            return seconds.ToString() + "s";
        }
    }

    public void Set()
    {
        gameObject.SetActive(false);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        IsClicked = false;
    }

    public void RegisterClickable()
    {
        ClickableManager.AddClickable(this);
    }
}
