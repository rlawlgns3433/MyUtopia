using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UiUpgradeCurrency : MonoBehaviour
{
    public Image imageCurrency;
    public TextMeshProUGUI textCurrencyForUpgrade;

    public void SetCurrency(Sprite sprite, BigNumber currency, bool preserveAspect = true, Image.Type type = Image.Type.Simple)
    {
        imageCurrency.sprite = sprite;
        textCurrencyForUpgrade.text = currency.ToString();

        imageCurrency.type = type;

        if (imageCurrency.type == Image.Type.Simple)
        {
            imageCurrency.preserveAspect = preserveAspect;
        }
    }
}
