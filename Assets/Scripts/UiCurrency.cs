using System.Numerics;
using TMPro;

public class UiCurrency : Observer
{
    private TextMeshProUGUI currency;
    public CurrencyType currencyType;
    public string format;

    private void Awake()
    {
        currency = GetComponent<TextMeshProUGUI>();
    }
    public override void Notify(Subject subject)
    {
        currency.text = string.Format(format, BigIntegerExtensions.ToString(CurrencyManager.currency[(int)currencyType]));
    }
}