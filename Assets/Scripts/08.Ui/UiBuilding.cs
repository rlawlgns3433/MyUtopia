using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiBuildingInfo : MonoBehaviour
{
    private static readonly string levelFormat = "{0} / {1}";
    private static readonly string exchangeFormat = "{0} -> {1}";

    public TextMeshProUGUI textBuildingLevel;
    public Image buildingProfile;
    public TextMeshProUGUI textBuildingName;
    public TextMeshProUGUI textProceeds;
    public TextMeshProUGUI textExchange;
    public TextMeshProUGUI textCurrentExchangeRate;
    public TextMeshProUGUI textNextExchangeRate;
    public Button buttonLevelUp;
    public Building building;

    public bool Set(Building building)
    {
        var uiFloorInformation = GameObject.FindWithTag(Tags.FloorInformation).GetComponentInParent<UiFloorInformation>();

        if(uiFloorInformation == null)
            return false;

        foreach(var uiBuilding in uiFloorInformation.uiBuildings)
        {
            if (uiBuilding.building.BuildingStat.BuildingData.GetName().Equals(building.BuildingStat.BuildingData.GetName()))
                return false;
        }

        if (building.BuildingStat.BuildingData.Building_ID == 0)
            return false;

        this.building = building;

        buttonLevelUp.onClick.AddListener(building.LevelUp);
        buttonLevelUp.onClick.AddListener(SetBuildingUi);

        SetBuildingUi();

        return true;
    }

    public void SetBuildingUi()
    {
        textBuildingLevel.text = string.Format(levelFormat, building.BuildingStat.BuildingData.Level, building.BuildingStat.BuildingData.Level_Max);
        textBuildingName.text = building.BuildingStat.BuildingData.GetName();
        //uiBuildingInfo.buildingProfile.sprite = building.BuildingData.GetProfile();
        textProceeds.text = ((CurrencyType)building.BuildingStat.Resource_Type).ToString();
        textExchange.text = string.Format(exchangeFormat, ((CurrencyType)building.BuildingStat.Materials_Type), ((CurrencyType)building.BuildingStat.Resource_Type));
        textCurrentExchangeRate.text = building.BuildingStat.Conversion_rate.ToString();

        if (building.BuildingStat.Level < building.BuildingStat.Level_Max)
        {
            textNextExchangeRate.text = DataTableMgr.GetBuildingTable().Get(building.BuildingStat.Building_ID + 100).Conversion_rate.ToString();
        }
        else
        {
            textNextExchangeRate.text = string.Empty;
        }
    }
}