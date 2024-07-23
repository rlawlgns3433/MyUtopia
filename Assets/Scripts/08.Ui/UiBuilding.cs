using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiBuildingInfo : MonoBehaviour
{
    private static readonly string lvFormat = "Lv.{0}";
    public TextMeshProUGUI textBuildingLevel;
    public Image buildingProfile;
    public TextMeshProUGUI textBuildingName;
    public TextMeshProUGUI textDescription;
    public Button buttonLevelUp;
    public Building building;
    public UiUpgradeCurrency uiBuildingUpgradeCurrency;
    public Transform contents; // ������ ���׷��̵� �� ��ȭ�� �󸶳� �ʿ��� ��

    public bool Set(Building building)
    {
        var uiFloorInformation = UiManager.Instance.floorInformationUi;

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

    public async void SetBuildingUi()
    {
        textBuildingLevel.text = string.Format(lvFormat, building.BuildingStat.Level);
        textBuildingName.text = building.BuildingStat.BuildingData.GetName();

        if(building.BuildingStat.Level_Up_Coin_Value != "0")
        {
            var currency = Instantiate(uiBuildingUpgradeCurrency, contents);
            var sprite = await DataTableMgr.GetResourceTable().Get((int)CurrencyType.Coin).GetImage();
            var value = building.BuildingStat.Level_Up_Coin_Value.ToBigNumber();
            currency.SetCurrency(sprite, value);
        }

        if(building.BuildingStat.Level_Up_Resource_1 != 0)
        {
            var currency = Instantiate(uiBuildingUpgradeCurrency, contents);
            var sprite = await DataTableMgr.GetResourceTable().Get(building.BuildingStat.Level_Up_Resource_1).GetImage();
            var value = building.BuildingStat.Resource_1_Value.ToBigNumber();
            currency.SetCurrency(sprite, value);
        }
        
        if(building.BuildingStat.Level_Up_Resource_2 != 0)
        {
            var currency = Instantiate(uiBuildingUpgradeCurrency, contents);
            var sprite = await DataTableMgr.GetResourceTable().Get(building.BuildingStat.Level_Up_Resource_2).GetImage();
            var value = building.BuildingStat.Resource_2_Value.ToBigNumber();
            currency.SetCurrency(sprite, value);
        }
        
        if(building.BuildingStat.Level_Up_Resource_3 != 0)
        {
            var currency = Instantiate(uiBuildingUpgradeCurrency, contents);
            var sprite = await DataTableMgr.GetResourceTable().Get(building.BuildingStat.Level_Up_Resource_3).GetImage();
            var value = building.BuildingStat.Resource_3_Value.ToBigNumber();
            currency.SetCurrency(sprite, value);
        }


        //uiBuildingInfo.buildingProfile.sprite = building.BuildingData.GetProfile();
        //textDescription.text = �ǹ� ����
    }
}