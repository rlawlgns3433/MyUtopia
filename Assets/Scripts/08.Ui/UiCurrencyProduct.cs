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
    private TextMeshProUGUI textCurrencyName;
    [SerializeField]
    private Image image;

    public CurrencyProductType currencyType;

    public void Start()
    {
        textCurrencyName.text = DataTableMgr.GetResourceTable().Get((int)currencyType).GetName();
    }

    public override void Notify(Subject subject)
    {
        textCurrency.text = CurrencyManager.product[currencyType].ToString();
    }
}