using System.Diagnostics;
using System.Numerics;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UiCurrencyProduct : Observer
{
    [SerializeField]
    private TextMeshProUGUI textCurrency;
    [SerializeField]
    private Image image;

    public CurrencyProductType currencyType;

    public override void Notify(Subject subject)
    {
        textCurrency.text = CurrencyManager.product[currencyType].ToString();
    }
}