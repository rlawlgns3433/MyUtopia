using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UiBuildingInfo
{
    public UiBuildingInfo()
    {
        textBuildingLevel = new TextMeshProUGUI();
        textBuildingName = new TextMeshProUGUI();
        textProceeds = new TextMeshProUGUI();
        textExchange = new TextMeshProUGUI();
        textCurrentExchangeRate = new TextMeshProUGUI();
        textNextExchangeRate = new TextMeshProUGUI();
    }

    public TextMeshProUGUI textBuildingLevel;
    public Image buildingProfile;
    public TextMeshProUGUI textBuildingName;
    public TextMeshProUGUI textProceeds;
    public TextMeshProUGUI textExchange;
    public TextMeshProUGUI textCurrentExchangeRate;
    public TextMeshProUGUI textNextExchangeRate;
    public Button buttonLevelUp;
}