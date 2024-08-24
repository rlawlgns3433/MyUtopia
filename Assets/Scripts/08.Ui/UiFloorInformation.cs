using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UiFloorInformation : MonoBehaviour
{
    private static readonly string levelFormat = "{0} / {1}";

    public UiBuildingInfo buildingInfoPrefab;
    public Transform buildingParent;
    public TextMeshProUGUI textFloorName;
    public UiFloorInfoBlock uiFloorInfoBlock;

    public List<UiBuildingInfo> uiBuildings;
    private ResourceTable resourceTable;
    public Floor currentFloor;
    public FloorStat floorStat;

    private void Awake()
    {
        uiBuildings = new List<UiBuildingInfo>();
        resourceTable = DataTableMgr.GetResourceTable();
    }

    public void SetFloorData()
    {
        var floor = FloorManager.Instance.GetCurrentFloor();

        if (floor == null)
            return;

        currentFloor = floor;
        floorStat = currentFloor.FloorStat;

        SetFloorUi();
    }

    public void SetFloorData(string floorId)
    {
        if (!FloorManager.Instance.floors.ContainsKey(floorId))
            return;

        currentFloor = FloorManager.Instance.GetFloor(floorId);
        floorStat = currentFloor.FloorStat;

        SetFloorUi();
    }


    public void SetFloorUi()
    {
        textFloorName.text = floorStat.FloorData.GetFloorName();

        uiFloorInfoBlock.Set(currentFloor);

        if (currentFloor.FloorStat.Floor_Num <= 2)
        {
            SetActiveFalseAllBuildingFurniture();
            return;
        }
        RefreshBuildingFurnitureData();
    }

    public bool ValidateBuildingData(Building newBuilding)
    {
        bool isSucceed = true;

        foreach (var uiBuilding in uiBuildings)
        {
            if (uiBuilding.building.BuildingStat.Floor_Type != currentFloor.FloorStat.Floor_Type)
                uiBuilding.gameObject.SetActive(false);

            if (uiBuilding.building.BuildingStat.BuildingData.GetName().Equals(newBuilding.BuildingStat.BuildingData.GetName()))
            {
                uiBuilding.gameObject.SetActive(true);
                uiBuilding.SetFinishUi();
                isSucceed = false;
            }
        }

        return isSucceed;
    }

    public void RefreshBuildingFurnitureData()
    {
        foreach (var building in currentFloor.buildings)
        {
            if (!building.BuildingStat.IsLock)
            {
                if (ValidateBuildingData(building))
                {
                    UiBuildingInfo uiBuildingInfo = Instantiate(buildingInfoPrefab, buildingParent);
                    bool isSucceed = uiBuildingInfo.Set(building);

                    if (isSucceed)
                    {
                        uiBuildings.Add(uiBuildingInfo);
                    }
                }
            }
        }
    }

    public void SetActiveFalseAllBuildingFurniture()
    {
        foreach (var uiBuilding in uiBuildings)
        {
            uiBuilding.gameObject.SetActive(false);
        }
    }
}
