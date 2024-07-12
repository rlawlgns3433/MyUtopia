using System.Collections;
using TMPro;
using UnityEditor.Build.Pipeline.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class UiBuildingInfo : MonoBehaviour
{
    private static readonly string levelFormat = "{0} / {1}";
    private static readonly string exchangeFormat = "{0} -> {1}";

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
    public BuildingData buildingData;

    public bool Set(BuildingData newData)
    {
        var uiFloorInformation = GameObject.FindWithTag(Tags.FloorInformation).GetComponentInParent<UiFloorInformation>();

        if(uiFloorInformation == null)
            return false;

        foreach(var uiBuilding in uiFloorInformation.uiBuildings)
        {
            if (uiBuilding.buildingData.ID == newData.ID)
                return false;
        }

        if (newData.ID == 0)
            return false;
        buildingData = newData;
        textBuildingLevel.text = string.Format(levelFormat, buildingData.Level, buildingData.Level_Max);
        textBuildingName.text = buildingData.GetName();
        //uiBuildingInfo.buildingProfile.sprite = building.BuildingData.GetProfile();
        textProceeds.text = ((CurrencyType)buildingData.Resource_Type).ToString();
        textExchange.text = string.Format(exchangeFormat, ((CurrencyType)buildingData.Materials_Type), ((CurrencyType)buildingData.Resource_Type));
        textCurrentExchangeRate.text = buildingData.Conversion_rate.ToString();

        if (buildingData.Level < buildingData.Level_Max)
        {
            textNextExchangeRate.text = DataTableMgr.GetBuildingTable().Get(buildingData.ID + 100).Conversion_rate.ToString();
        }
        else
        {
            textNextExchangeRate.text = string.Empty;
        }

        return true;
    }


}