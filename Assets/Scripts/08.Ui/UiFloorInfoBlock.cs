using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UiFloorInfoBlock : MonoBehaviour, IUISetupable, IGrowable
{
    private static readonly string lvFormat = "Lv.{0} / Lv.{1}";
    public List<Image> imageProductions = new List<Image>();
    public TextMeshProUGUI textLevel;
    public TextMeshProUGUI textMax;
    public TextMeshProUGUI textFloorDesc;
    public UiUpgradeCurrency uiUpgradeCurrency;
    public List<UiUpgradeCurrency> uiUpgradeCurrencies = new List<UiUpgradeCurrency>();
    public Button buttonLevelUp;
    public Floor floor;
    public Transform contents;
    public ClockFormatTimer clockFormatTimer;

    public bool IsUpgrading { get => floor.FloorStat.IsUpgrading; set => floor.FloorStat.IsUpgrading = value; }
    public float UpgradeStartTime { get => floor.FloorStat.UpgradeStartTime; set => floor.FloorStat.UpgradeStartTime = value; }

    private void Start()
    {
        SetTimerWhenStartUp();
    }

    private void SetTimerWhenStartUp()
    {
        if (!IsUpgrading)
            return;
        clockFormatTimer.canStartTimer = true;
        clockFormatTimer.timerText.gameObject.SetActive(true);
        clockFormatTimer.SetTimer(floor.FloorStat.UpgradeTimeLeft - Mathf.FloorToInt(DateTime.UtcNow.Hour * 3600 + DateTime.UtcNow.Minute * 60 + DateTime.UtcNow.Second - floor.FloorStat.UpgradeStartTime));

        foreach (var currency in uiUpgradeCurrencies)
        {
            Destroy(currency.gameObject);
        }
        uiUpgradeCurrencies.Clear();
        textMax.gameObject.SetActive(false);
        clockFormatTimer.StartClockTimer();
    }

    public void FinishUpgrade()
    {
        clockFormatTimer.timerText.gameObject.SetActive(false);
    }

    public void LevelUp()
    {
        //foreach (var floor in FloorManager.Instance.floors.Values)
        //{
            if (floor.FloorStat.IsUpgrading)
            {
                floor.LevelUp();
                //this.floor = floor;
                return;
            }
        //}
    }

    public bool Set(Floor floor)
    {
        foreach (var production in imageProductions)
        {
            production.sprite = null;
        }

        foreach (var currency in uiUpgradeCurrencies)
        {
            Destroy(currency.gameObject);
        }
        uiUpgradeCurrencies.Clear();

        var uiFloorInformation = UiManager.Instance.floorInformationUi;

        if (uiFloorInformation == null)
            return false;

        this.floor = floor;

        if (floor.FloorStat.FloorData.Floor_ID == 0)
            return false;

        buttonLevelUp.onClick.RemoveAllListeners();
        buttonLevelUp.onClick.AddListener(SetStartUi);
        buttonLevelUp.onClick.AddListener(clockFormatTimer.StartClockTimer);

        if (IsUpgrading)
        {
            floor.FloorStat.UpgradeTimeLeft = floor.FloorStat.Level_Up_Time - Mathf.CeilToInt(DateTime.UtcNow.Hour * 3600 + DateTime.UtcNow.Minute * 60 + DateTime.UtcNow.Second - UpgradeStartTime);
            clockFormatTimer.SetTimer(floor.FloorStat.UpgradeTimeLeft);
            clockFormatTimer.timerText.gameObject.SetActive(true);
            textLevel.text = string.Format(lvFormat, floor.FloorStat.Grade, floor.FloorStat.Grade_Max);
            textFloorDesc.text = DataTableMgr.GetStringTable().Get(floor.FloorStat.FloorData.Floor_Desc);

            if (floor.FloorStat.UpgradeTimeLeft <= 0)
            {
                LevelUp();
                SetFinishUi();
                FinishUpgrade();

                clockFormatTimer.canStartTimer = true;
                clockFormatTimer.timerText.gameObject.SetActive(false);
                IsUpgrading = false;
            }

        }
        else
        {
            SetFinishUi();
            FinishUpgrade();
        }

        return true;
    }

    public async void SetFinishUi()
    {
        UiManager.Instance.floorInformationUi.RefreshBuildingFurnitureData();

        if (FloorManager.Instance.GetCurrentFloor() != floor)
            return;

        textLevel.text = string.Format(lvFormat, floor.FloorStat.Grade, floor.FloorStat.Grade_Max);
        textFloorDesc.text = DataTableMgr.GetStringTable().Get(floor.FloorStat.FloorData.Floor_Desc);

        for (int i = 0; i < floor.buildings.Count; i++)
        {
            var building = floor.buildings[i];
            if (building.BuildingStat.IsLock)
                continue;

            imageProductions[i].sprite = await building.BuildingStat.BuildingData.GetProfile();
            imageProductions[i].type = Image.Type.Sliced;
        }


        if (floor.FloorStat.Level_Up_Coin_Value != "0")
        {
            var currency = Instantiate(uiUpgradeCurrency, contents);
            var sprite = await DataTableMgr.GetResourceTable().Get((int)CurrencyType.Coin).GetImage();
            var value = floor.FloorStat.Level_Up_Coin_Value.ToBigNumber();
            currency.SetCurrency(sprite, value);
            uiUpgradeCurrencies.Add(currency);
        }

        if (floor.FloorStat.Level_Up_Resource_1 != 0)
        {
            var currency = Instantiate(uiUpgradeCurrency, contents);
            var sprite = await DataTableMgr.GetResourceTable().Get(floor.FloorStat.Level_Up_Resource_1).GetImage();
            var value = floor.FloorStat.Resource_1_Value.ToBigNumber();
            currency.SetCurrency(sprite, value);
            uiUpgradeCurrencies.Add(currency);
        }

        if (floor.FloorStat.Level_Up_Resource_2 != 0)
        {
            var currency = Instantiate(uiUpgradeCurrency, contents);
            var sprite = await DataTableMgr.GetResourceTable().Get(floor.FloorStat.Level_Up_Resource_2).GetImage();
            var value = floor.FloorStat.Resource_2_Value.ToBigNumber();
            currency.SetCurrency(sprite, value);
            uiUpgradeCurrencies.Add(currency);
        }

        if (floor.FloorStat.Level_Up_Resource_3 != 0)
        {
            var currency = Instantiate(uiUpgradeCurrency, contents);
            var sprite = await DataTableMgr.GetResourceTable().Get(floor.FloorStat.Level_Up_Resource_3).GetImage();
            var value = floor.FloorStat.Resource_3_Value.ToBigNumber();
            currency.SetCurrency(sprite, value);
            uiUpgradeCurrencies.Add(currency);
        }


        if (floor.FloorStat.Grade == floor.FloorStat.Grade_Max)
        {
            textMax.gameObject.SetActive(true);
            return;
        }
        textMax.gameObject.SetActive(false);

        //textDescription.text = 건물 설명
    }

    public void SetStartUi()
    {
        clockFormatTimer.canStartTimer = false;

        if (floor.FloorStat.IsUpgrading)
        {
            return;
        }

        if (!CheckUpgradeCondition())
        {
            // 모든 건물이 레벨을 충족하지 못함
            return;
        }

        if (!floor.CheckCurrency())
        {
            return;
        }

        clockFormatTimer.canStartTimer = true;
        IsUpgrading = true;
        UpgradeStartTime = DateTime.UtcNow.Hour * 3600 + DateTime.UtcNow.Minute * 60 + DateTime.UtcNow.Second;

        floor.SpendCurrency();

        clockFormatTimer.timerText.gameObject.SetActive(true);

        clockFormatTimer.SetTimer(floor.FloorStat.Level_Up_Time);

        foreach (var currency in uiUpgradeCurrencies)
        {
            Destroy(currency.gameObject);
        }
        uiUpgradeCurrencies.Clear();
    }

    public bool CheckUpgradeCondition()
    {
        // 계층 업그레이드 조건 추가
        // 계층의 락 해제된 건물들이 Grade_Level_Max

        foreach (var building in floor.buildings)
        {
            if (building.BuildingStat.IsLock)
                continue;

            if (building.BuildingStat.Level < floor.FloorStat.Grade_Level_Max)
            {
                return false;
            }
        }

        return true;
    }
}
