using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StorageSlotUi : MonoBehaviour
{
    public TextMeshProUGUI slotValue;
    public Image typeImage;

    public void SetText(string text)
    {
        slotValue.text = text;
    }

    public async UniTask SetSprite(CurrencyProductType currencyType)
    {
        var currencySprite = await DataTableMgr.GetResourceTable().Get((int)currencyType).GetImage();
        typeImage.sprite = currencySprite;
    }
}
