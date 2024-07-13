using System.Diagnostics;
using System.Numerics;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UiCurrency : Observer
{
    [SerializeField]
    private TextMeshProUGUI textCurrency;
    [SerializeField]
    private Image image;

    public CurrencyType currencyType;

    public override void Notify(Subject subject)
    {
        textCurrency.text = CurrencyManager.currency[(int)currencyType].ToString();
    }
}