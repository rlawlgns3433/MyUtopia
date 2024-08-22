using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiConfirmPanel : MonoBehaviour
{
    private static readonly string format = "{0}다이아를 지불하고 즉시 완성할까요?";
    public Button buttonConfirm;
    public Button buttonCancel;
    public TextMeshProUGUI textContext;
    public BigNumber price;
    public IGrowable growable;

    public void SetText(BigNumber price, IGrowable growable)
    {
        this.price = price;
        this.growable = growable;
        textContext.text = string.Format(format, price.ToString());
    }

    public void OnClickConfirm()
    {
        if(CheckCurrency())
        {
            growable.LevelUp();
            growable.IsUpgrading = false;
            UseCurrency();
        }
        else
        {
            // 경고 UI

        }
        UiManager.Instance.ShowMainUi();
    }

    public bool CheckCurrency()
    {
        if (CurrencyManager.currency[(CurrencyType.Diamond)] >= price)
            return true;

        return false;
    }

    public void UseCurrency()
    {
        CurrencyManager.currency[(CurrencyType.Diamond)] -= price;
    }
}
