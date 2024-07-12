using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;



//public class UiFacilityInfo
//{
//    public TextMeshProUGUI textBuildingLevel;
//    public Image buildingProfile;
//    public TextMeshProUGUI textBuildingName;
//    public TextMeshProUGUI textAccumTime;
//    public TextMeshProUGUI textCurrentSaveTime;
//    public TextMeshProUGUI textNextSaveTime;
//    public Button buttonLevelUp;
//}


public class UiFloorInformation : MonoBehaviour
{
    private static readonly string levelFormat = "{0} / {1}";
    private static readonly string exchangeFormat = "{0} -> {1}";

    public UiBuildingInfo buildingInfoPrefab;

    public TextMeshProUGUI textFloorName = new TextMeshProUGUI();
    public TextMeshProUGUI textFloorLevel = new TextMeshProUGUI();
    public List<TextMeshProUGUI> textProduceCurrencies;
    public Button buttonLevelUp;
    public TextMeshProUGUI textSynergyName = new TextMeshProUGUI();
    public TextMeshProUGUI textFacilityEffectName = new TextMeshProUGUI();
    public List<UiBuildingInfo> uiBuildings;
    //public List<UiBuildingInfo> uiFacilities;

    public Floor currentFloor;
    public FloorData floorData;

    private void Awake()
    {
        uiBuildings = new List<UiBuildingInfo>();
        //uiFacilities = new List<UiBuildingInfo>();
        textProduceCurrencies = new List<TextMeshProUGUI>();
    }

    public void SetFloorData(string floorId)
    {
        if (!FloorManager.Instance.floors.ContainsKey(floorId))
            return;

        currentFloor = FloorManager.Instance.floors[floorId];
        floorData = currentFloor.FloorData;

        SetFloorUi();
    }

    public void SetFloorUi()
    {
        textFloorName.text = floorData.GetFloorName();
        textFloorLevel.text = string.Format(levelFormat, floorData.Grade, floorData.Grade_Max);

        foreach (var building in currentFloor.buildings)
        {
            if(!building.isLock)
            {
                TextMeshProUGUI textCurrency = new TextMeshProUGUI();
                textCurrency.text = building.buildingType.ToString();
                textProduceCurrencies.Add(textCurrency);

                /*
                건물 이름
                생산품
                재료, 생산
                현재 교환비
                다음 교환비 // 현재 최고 레벨이 아닌경우민


                    public TextMeshProUGUI textBuildingLevel;
                    public Image buildingProfile;
                    public TextMeshProUGUI textBuildingName;
                    public TextMeshProUGUI textProceeds;
                    public TextMeshProUGUI textExchange;
                    public TextMeshProUGUI textCurrentExchangeRate;
                    public TextMeshProUGUI textNextExchangeRate;
                    public Button buttonLevelUp;
                 */

                UiBuildingInfo uiBuildingInfo = new UiBuildingInfo();
                uiBuildingInfo.textBuildingLevel.text = string.Format(levelFormat, building.BuildingData.Level, building.BuildingData.Level_Max);
                //uiBuildingInfo.buildingProfile.sprite = building.BuildingData.GetProfile();
                uiBuildingInfo.textProceeds.text = ((CurrencyType)building.BuildingData.Resource_Type).ToString();
                uiBuildingInfo.textExchange.text = string.Format(exchangeFormat, ((CurrencyType)building.BuildingData.Materials_Type), ((CurrencyType)building.BuildingData.Resource_Type));
                uiBuildingInfo.textCurrentExchangeRate.text = building.BuildingData.Conversion_rate.ToString();

                if(building.BuildingData.Level < building.BuildingData.Level_Max)
                {
                    uiBuildingInfo.textNextExchangeRate.text = DataTableMgr.GetBuildingTable().Get(building.BuildingData.ID + 1).Conversion_rate.ToString();
                }
                else
                {
                    uiBuildingInfo.textNextExchangeRate.text = string.Empty;
                }

                uiBuildings.Add(uiBuildingInfo);
            }
        }

        buttonLevelUp.onClick.AddListener(currentFloor.LevelUp);

        //textSynergyName;
        //textFacilityEffectName;
        //uiBuildings;
        //uiFacilities;

    }
}
