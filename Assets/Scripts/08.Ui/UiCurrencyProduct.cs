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

    public async void Start()
    {
        image.sprite = await DataTableMgr.GetResourceTable().Get((int)currencyType).GetImage();
    }

    public override void Notify(Subject subject)
    {
        textCurrency.text = CurrencyManager.product[currencyType].ToString();
    }
}