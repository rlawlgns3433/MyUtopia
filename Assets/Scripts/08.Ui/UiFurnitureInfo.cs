using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiFurnitureInfo : MonoBehaviour, IUISetupable, IGrowable
{
    private static readonly string lvFormat = "Lv.{0}";
    public TextMeshProUGUI textFurnitureLevel;
    public Image furnitureProfile;
    public TextMeshProUGUI textFurnitureName;
    public TextMeshProUGUI textDescription;
    public TextMeshProUGUI textMax;
    public Button buttonLevelUp;
    public Building building;
    public List<UiUpgradeCurrency> uiUpgradeCurrencies = new List<UiUpgradeCurrency>();
    public UiUpgradeCurrency uiUpgradeCurrency;
    public Transform contents; // 하위에 업그레이드 시 재화가 얼마나 필요한 지
    public ClockFormatTimer clockFormatTimer;
    public bool IsUpgrading { get => building.IsUpgrading; set => building.IsUpgrading = value; }

    public void FinishUpgrade()
    {
        clockFormatTimer.timerText.gameObject.SetActive(false);
    }

    public void LevelUp()
    {
        building.LevelUp(); // 두 번 호출 가능성 있음
    }

    public bool Set(Building building)
    {
        foreach (var currency in uiUpgradeCurrencies)
        {
            Destroy(currency.gameObject);
        }
        uiUpgradeCurrencies.Clear();

        var uiFloorInformation = UiManager.Instance.floorInformationUi;

        if (uiFloorInformation == null)
            return false;

        foreach (var uiFurniture in uiFloorInformation.uiFurnitures)
        {
            if (uiFurniture.building.BuildingStat.BuildingData.GetName().Equals(building.BuildingStat.BuildingData.GetName()))
                return false;
        }

        if (building.BuildingStat.BuildingData.Building_ID == 0)
            return false;

        this.building = building;

        buttonLevelUp.onClick.RemoveAllListeners();
        buttonLevelUp.onClick.AddListener(SetStartUi);
        buttonLevelUp.onClick.AddListener(clockFormatTimer.StartClockTimer);
        if(building.IsUpgrading)
        {
            clockFormatTimer.timerText.gameObject.SetActive(true);
        }
        else
        {
            SetFinishUi();
        }

        return true;
    }

    public async void SetFinishUi()
    {

        textFurnitureLevel.text = string.Format(lvFormat, building.BuildingStat.Level);
        textFurnitureName.text = building.BuildingStat.BuildingData.GetName();
        textDescription.text = building.BuildingStat.BuildingData.GetDescription();
        furnitureProfile.sprite = await building.BuildingStat.BuildingData.GetProfile();


        if (building.BuildingStat.Level_Up_Coin_Value != "0")
        {
            var currency = Instantiate(uiUpgradeCurrency, contents);
            var sprite = await DataTableMgr.GetResourceTable().Get((int)CurrencyType.Coin).GetImage();
            var value = building.BuildingStat.Level_Up_Coin_Value.ToBigNumber();
            currency.SetCurrency(sprite, value);
            uiUpgradeCurrencies.Add(currency);
        }

        if (building.BuildingStat.Level == building.BuildingStat.Level_Max)
        {
            textMax.gameObject.SetActive(true);
            buttonLevelUp.interactable = false;
            return;
        }
        else
        {
            textMax.gameObject.SetActive(false);
            buttonLevelUp.interactable = true;
        }

        //uiBuildingInfo.buildingProfile.sprite = building.FurnitureData.GetProfile();
        //textDescription.text = 건물 설명
    }

    public void SetStartUi()
    {
        if (!building.CheckCurrency())
        {
            clockFormatTimer.canStartTimer = false;
            return;
        }

        clockFormatTimer.canStartTimer = true;

        IsUpgrading = true;

        building.SpendCurrency();

        clockFormatTimer.timerText.gameObject.SetActive(true);

        clockFormatTimer.SetTimer(/*building.BuildingStat.Level_Up_Time*/10);

        foreach (var currency in uiUpgradeCurrencies)
        {
            Destroy(currency.gameObject);
        }

        uiUpgradeCurrencies.Clear();
    }
}
