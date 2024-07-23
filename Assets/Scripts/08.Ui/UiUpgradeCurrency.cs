using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UiUpgradeCurrency : MonoBehaviour
{
    public Image imageCurrency;
    public TextMeshProUGUI textCurrencyForUpgrade;

    public void SetCurrency(Sprite sprite, BigNumber currency)
    {
        imageCurrency.sprite = sprite;
        textCurrencyForUpgrade.text = currency.ToString();
        imageCurrency.preserveAspect = true;
    }
}
