using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class UiFloorInformation : MonoBehaviour
{
    private static readonly string levelFormat = "{0} / {1}";
    private static readonly string exchangeFormat = "{0} -> {1}";

    public UiBuildingInfo buildingInfoPrefab;
    public Transform buildingParent;

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

                if(ValidateBuildingData(building.BuildingData))
                {
                    UiBuildingInfo uiBuildingInfo = Instantiate(buildingInfoPrefab, buildingParent);
                    bool isSucceed = uiBuildingInfo.Set(building.BuildingData);

                    if (isSucceed)
                        uiBuildings.Add(uiBuildingInfo);
                }
            }
        }

        buttonLevelUp.onClick.AddListener(currentFloor.LevelUp);
    }

    public bool ValidateBuildingData(BuildingData newData)
    {
        foreach (var uiBuilding in uiBuildings)
        {
            if (uiBuilding.buildingData.ID == newData.ID)
                return false;
        }
        return true;
    }
}
