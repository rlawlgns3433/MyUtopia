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
    //public UiAnimalSynergyBlock synergyEffectInfoPrefab; 시너지
    //public UiFurnitureEffectBlock furnitureEffectPrefab;

    public Transform furnitureParent;
    public Transform buildingParent;
    public Transform furnitureEffectParent;
    public Transform synergyEffectParent;

    public TextMeshProUGUI textFloorName;

    public UiFloorInfoBlock uiFloorInfoBlock;

    public List<UiBuildingInfo> uiBuildings;
    public List<UiFurnitureInfo> uiFurnitures;
    //public List<UiAnimalSynergyBlock> uiAnimalSynergyEffects; 시너지
    //public List<UiFurnitureEffectBlock> uiFurnitureEffects;

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
        //foreach (var uiBuilding in uiBuildings)
        //{
        //    Destroy(uiBuilding.gameObject);
        //}
        //uiBuildings.Clear();

        //foreach (var uiFurniture in uiFurnitures)
        //{
        //    Destroy(uiFurniture.gameObject);
        //}
        //uiFurnitures.Clear();

        //foreach (var uiSynergyEffect in uiAnimalSynergyEffects) 시너지
        //{
        //    Destroy(uiSynergyEffect.gameObject);
        //}
        //uiAnimalSynergyEffects.Clear();
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

        //foreach (var synergyEffect in currentFloor.synergyStats) 시너지
        //{
        //    UiAnimalSynergyBlock uiAnimalSynergyBlock = Instantiate(synergyEffectInfoPrefab, synergyEffectParent);
        //    uiAnimalSynergyBlock.Set(synergyEffect);
        //    uiAnimalSynergyEffects.Add(uiAnimalSynergyBlock);
        //}
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
                isSucceed = false;
            }
        }

        return isSucceed;
    }

    public bool ValidateFurnitureData(Building newBuilding)
    {
        bool isSucceed = true;
        foreach (var uiFurniture in uiFurnitures)
        {
            if (uiFurniture.building.BuildingStat.BuildingData.Floor_Type != currentFloor.FloorStat.Floor_Type)
                uiFurniture.gameObject.SetActive(false);

            if (uiFurniture.building.BuildingStat.BuildingData.GetName().Equals(newBuilding.BuildingStat.BuildingData.GetName()))
            {
                uiFurniture.gameObject.SetActive(true);
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

        foreach (var furniture in currentFloor.furnitures)
        {
            if (ValidateFurnitureData(furniture))
            {
                UiFurnitureInfo uiFurnitureInfo = Instantiate(furnitureInfoPrefab, furnitureParent);
                bool isSucceed = uiFurnitureInfo.Set(furniture);

                if (isSucceed)
                {
                    uiFurnitures.Add(uiFurnitureInfo);
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

        foreach (var uiFurniture in uiFurnitures)
        {
            uiFurniture.gameObject.SetActive(false);
        }
    }
}
