using System.Diagnostics;
using System.Numerics;
using UnityEngine;
using TMPro;

public class UiCurrency : Observer
{
    private TextMeshProUGUI currency;
    public CurrencyType currencyType;

    private void Awake()
    {
        currency = GetComponent<TextMeshProUGUI>();
    }
    public override void Notify(Subject subject)
    {
        currency.text = CurrencyManager.currency[(int)currencyType].ToString();
    }
}