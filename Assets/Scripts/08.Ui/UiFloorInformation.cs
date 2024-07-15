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

    public FloorStat floorStat;

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
        floorStat = currentFloor.FloorStat;

        SetFloorUi();
    }

    public void SetFloorUi()
    {
        textFloorName.text = floorStat.FloorData.GetFloorName();
        textFloorLevel.text = string.Format(levelFormat, floorStat.Grade, floorStat.Grade_Max);

        foreach (var building in currentFloor.buildings)
        {
            if(!building.isLock)
            {
                if(ValidateBuildingData(building))
                {
                    UiBuildingInfo uiBuildingInfo = Instantiate(buildingInfoPrefab, buildingParent);
                    bool isSucceed = uiBuildingInfo.Set(building);

                    if (isSucceed)
                    {
                        uiBuildings.Add(uiBuildingInfo);
                        //imageProduction[unlockBuildingCount++].sprite = resourceTable.Get(building.BuildingData.Resource_Type).GetImage();
                    }
                }
            }
        }
    }

    public bool ValidateBuildingData(Building newBuilding)
    {
        foreach (var uiBuilding in uiBuildings)
        {
            if (uiBuilding.building.BuildingData.GetName().Equals(newBuilding.BuildingData.GetName()))
                return false;
        }
        return true;
    }
}
