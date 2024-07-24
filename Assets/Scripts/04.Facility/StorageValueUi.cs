using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StorageValueUi : MonoBehaviour
{
    public List<CurrencyType> currencyTypes;
    public Image[] currencyImages;
    private BigNumber[] currentValue;
    private BigNumber[] currentWorkLoads;
    public GameObject[] currencyValues;
    private int maxValue;
    private int totalValue;
    private StorageConduct storage;
    public TextMeshProUGUI timeDifferenceText;

    private void OnEnable()
    {
        storage = GetComponentInParent<StorageConduct>();
        currentValue = new BigNumber[storage.CurrArray.Length];
        currentWorkLoads = new BigNumber[storage.CurrArray.Length];
        currencyTypes = storage.currencyTypes;
        maxValue = storage.MaxSeconds;
        totalValue = storage.CurrentTotalSeconds;
        var maxValueText = ConvertSecondsToHours(maxValue);
        var totalValueText = ConvertSecondsToHours(totalValue);
        timeDifferenceText.text = $"{totalValueText} / {maxValueText}";
        for(int i = 0; i < storage.CurrArray.Length; i++)
        {
            currentValue[i] = storage.CurrArray[i];
            currentWorkLoads[i] = storage.Values[i];
        }
        for (int i = 0; i < storage.CurrArray.Length; i++)
        {
            currencyValues[i].gameObject.SetActive(true);
            SetSprite(i).Forget();
            var currencyValueText = currencyValues[i].GetComponentInChildren<TextMeshProUGUI>();
            currencyValueText.text = currentValue[i].ToString();
            var currencyValueSlider = currencyValues[i].GetComponentInChildren<Slider>();
            var maxSeconds = maxValue / 3;
            if(currentValue[i] > 0)
            {
                currentWorkLoads[i] *= maxSeconds;
                var clampValue = BigNumber.ToFloatClamped01(currentValue[i], currentWorkLoads[i]);
                
                currencyValueSlider.value = clampValue;
                Debug.Log("test" + currencyValueSlider.value);
            }
            else
            {
                currencyValueSlider.value = 0;
            }
        }
    }

    private async UniTask SetSprite(int index)
    {
        var currencySprite = await DataTableMgr.GetResourceTable().Get((int)currencyTypes[index]).GetImage();
        currencyImages[index].sprite = currencySprite;
    }
    public void ExitUi()
    {
        UiManager.Instance.ShowMainUi();
    }
    public string ConvertSecondsToHours(int seconds)
    {
        if (seconds >= 3600)
        {
            double hours = seconds / 3600.0;
            double truncatedHours = Math.Truncate(hours * 10) / 10;
            return truncatedHours.ToString("0.#") + "h";
        }
        else if (seconds >= 60)
        {
            double minutes = seconds / 60.0;
            double truncatedMinutes = Math.Truncate(minutes * 10) / 10;
            return truncatedMinutes.ToString("0.#") + "m";
        }
        else
        {
            return seconds.ToString() + "s";
        }
    }
}
