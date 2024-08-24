using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UiFloorInformation : MonoBehaviour
{
    private static readonly string levelFormat = "{0} / {1}";

    public UiBuildingInfo buildingInfoPrefab;
    public Transform buildingParent;
    public TextMeshProUGUI textFloorName;

    public List<UiFloorInfoBlock> uiFloorInfoBlocks;
    public List<UiBuildingInfo> uiBuildings;
    private ResourceTable resourceTable;
    private Floor currentFloor;
    public Floor CurrentFloor
    {
        get
        {
            currentFloor = FloorManager.Instance.GetCurrentFloor();
            return currentFloor;
        }
        set
        {
            currentFloor = value;
        }
    }
    private FloorStat floorStat;
    public FloorStat FloorStat
    {
        get
        {
            floorStat = CurrentFloor.FloorStat;
            return floorStat;
        }
        set
        {
            floorStat = value;
        }
    }

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

        CurrentFloor = floor;
        floorStat = CurrentFloor.FloorStat;

        SetFloorUi();
    }

    public void SetFloorData(string floorId)
    {
        if (!FloorManager.Instance.floors.ContainsKey(floorId))
            return;

        CurrentFloor = FloorManager.Instance.GetFloor(floorId);
        floorStat = CurrentFloor.FloorStat;

        SetFloorUi();
    }


    public void SetFloorUi()
    {
        int index = floorStat.Floor_Num - 1;
        if (index >= 0 && index < uiFloorInfoBlocks.Count)
        {
            textFloorName.text = floorStat.FloorData.GetFloorName();
            uiFloorInfoBlocks[index].Set(CurrentFloor);
            RefreshFloorInfoBlock();
        }

        if (CurrentFloor.FloorStat.Floor_Num <= 2)
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
            if (uiBuilding.building.BuildingStat.Floor_Type != CurrentFloor.FloorStat.Floor_Type)
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
        foreach (var building in CurrentFloor.buildings)
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

    public void RefreshFloorInfoBlock()
    {
        for (int i = 0; i < uiFloorInfoBlocks.Count; i++)
        {
            if (floorStat.Floor_Num - 1 == i)
                uiFloorInfoBlocks[i].gameObject.SetActive(true);
            else
                uiFloorInfoBlocks[i].gameObject.SetActive(false);
        }
    }
}