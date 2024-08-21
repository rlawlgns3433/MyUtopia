using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiRequireCurrencyProduct : MonoBehaviour
{
    public int type;
    public string requireCount;
    public Image imageCurrencyProduct;
    public TextMeshProUGUI textCount;

    public async void SetData(int type, string requireCount)
    {
        this.type = type;
        this.requireCount = requireCount;
        // 이미지 로드 필요
        imageCurrencyProduct.sprite = await DataTableMgr.GetResourceTable().Get(type).GetImage();
        imageCurrencyProduct.type = Image.Type.Simple;
        imageCurrencyProduct.preserveAspect = true;

        textCount.text = new BigNumber(requireCount).ToString();
    }
}
