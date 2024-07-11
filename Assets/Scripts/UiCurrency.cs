using System.Diagnostics;
using System.Numerics;
using UnityEngine;
using TMPro;

public class UiCurrency : Observer
{
    [SerializeField]
    private TextMeshProUGUI textCurrency;
    public CurrencyType currencyType;

    public override void Notify(Subject subject)
    {
        textCurrency.text = CurrencyManager.currency[(int)currencyType].ToString();
    }
}