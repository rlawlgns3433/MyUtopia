using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;

public class UiFloorInformation : MonoBehaviour
{
    private static readonly string levelFormat = "{0} / {1}";
    private static readonly string exchangeFormat = "{0} -> {1}";

    public UiBuildingInfo buildingInfoPrefab;
    public Transform buildingParent;

    public TextMeshProUGUI textFloorName;
    public TextMeshProUGUI textFloorLevel;
    public TextMeshProUGUI textSynergyName;
    public TextMeshProUGUI textFacilityEffectName;
    public List<UiBuildingInfo> uiBuildings;
    public List<Image> imageProduction;

    private int unlockBuildingCount = 0;
    private ResourceTable resourceTable;
    //public List<UiBuildingInfo> uiFacilities;

    public Floor currentFloor;

    public FloorData floorData;

    private void Awake()
    {
        uiBuildings = new List<UiBuildingInfo>();
        resourceTable = DataTableMgr.GetResourceTable();
        //uiFacilities = new List<UiBuildingInfo>();
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
                if(ValidateBuildingData(building.BuildingData))
                {
                    UiBuildingInfo uiBuildingInfo = Instantiate(buildingInfoPrefab, buildingParent);
                    bool isSucceed = uiBuildingInfo.Set(building.BuildingData);

                    if (isSucceed)
                    {
                        uiBuildings.Add(uiBuildingInfo);
                        //imageProduction[unlockBuildingCount++].sprite = resourceTable.Get(building.BuildingData.Resource_Type).GetImage();
                    }
                }
            }
        }
    }

    public bool ValidateBuildingData(BuildingData newData)
    {
        foreach (var uiBuilding in uiBuildings)
        {
            if (uiBuilding.buildingData.Building_ID == newData.Building_ID)
                return false;
        }
        return true;
    }
}
