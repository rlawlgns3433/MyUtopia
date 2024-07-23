using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;

public class UiFloorInformation : MonoBehaviour
{
    private static readonly string levelFormat = "{0} / {1}";

    public UiBuildingInfo buildingInfoPrefab;
    public UiFurnitureInfo furnitureInfoPrefab;
    public Transform furnitureParent;
    public Transform buildingParent;

    public TextMeshProUGUI textFloorName;
    public TextMeshProUGUI textSynergyName;
    public TextMeshProUGUI textFacilityEffectName;

    public UiFloorInfoBlock uiFloorInfoBlock;
    public List<UiBuildingInfo> uiBuildings;
    public List<UiFurnitureInfo> uiFurnitures;

    private ResourceTable resourceTable;

    public Floor currentFloor;

    public FloorStat floorStat;

    private void Awake()
    {
        uiBuildings = new List<UiBuildingInfo>();
        resourceTable = DataTableMgr.GetResourceTable();
        uiFurnitures = new List<UiFurnitureInfo>();
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

        currentFloor = FloorManager.Instance.floors[floorId];
        floorStat = currentFloor.FloorStat;

        SetFloorUi();
    }

    public void ClearFloorUi()
    {
        foreach (var uiBuilding in uiBuildings)
        {
            Destroy(uiBuilding.gameObject);
        }
        uiBuildings.Clear();

        foreach(var uiFurniture in uiFurnitures)
        {
            Destroy(uiFurniture.gameObject);
        }
        uiFurnitures.Clear();
    }

    public void SetFloorUi()
    {
        textFloorName.text = floorStat.FloorData.GetFloorName();

        uiFloorInfoBlock.Set(currentFloor);

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
                    }
                }
            }
        }

        // 여기서 건물과 같이 시설물 추가

        foreach (var furniture in currentFloor.furnitures)
        {
            UiFurnitureInfo uiFurnitureInfo = Instantiate(furnitureInfoPrefab, furnitureParent);
            bool isSucceed = uiFurnitureInfo.Set(furniture);

            if (isSucceed)
            {
                uiFurnitures.Add(uiFurnitureInfo);
            }
        }
    }

    public bool ValidateBuildingData(Building newBuilding)
    {
        foreach (var uiBuilding in uiBuildings)
        {
            if (uiBuilding.building.BuildingStat.BuildingData.GetName().Equals(newBuilding.BuildingStat.BuildingData.GetName()))
                return false;
        }
        return true;
    }
}
