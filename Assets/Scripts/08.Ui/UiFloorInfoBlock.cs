using System;
using System.Collections.Generic;
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
    public UiUpgradeCurrency uiUpgradeCurrency;
    public List<UiUpgradeCurrency> uiUpgradeCurrencies = new List<UiUpgradeCurrency>();
    public Button buttonLevelUp;
    public Floor floor;
    public Transform contents;
    public ClockFormatTimer clockFormatTimer;

    public void FinishUpgrade()
    {
        clockFormatTimer.timerText.gameObject.SetActive(false);
    }

    public void LevelUp()
    {
        floor.LevelUp();
    }

    public bool Set(Floor floor)
    {
        foreach(var production in imageProductions)
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
        SetFinishUi();

        return true;
    }

    public async void SetFinishUi()
    {
        //UiManager.Instance.floorInformationUi.SetFloorData();

        textLevel.text = string.Format(lvFormat, floor.FloorStat.Grade, floor.FloorStat.Grade_Max);

        for (int i = 0; i < floor.buildings.Count; i++)
        {
            var building = floor.buildings[i];
            if (building.BuildingStat.IsLock)
                continue;

            imageProductions[i].sprite = await building.BuildingStat.BuildingData.GetProfile(); // 빌딩 이미지 추가 예정
            imageProductions[i].type = Image.Type.Sliced;
        }

        if(floor.FloorStat.Grade == floor.FloorStat.Grade_Max)
        {
            textMax.gameObject.SetActive(true);
            return;
        }
        textMax.gameObject.SetActive(false);

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

        //textDescription.text = 건물 설명
    }

    public void SetStartUi()
    {
        if (!floor.CheckCurrency())
        {
            clockFormatTimer.canStartTimer = false;
            return;
        }

        clockFormatTimer.canStartTimer = true;

        floor.SpendCurrency();

        clockFormatTimer.timerText.gameObject.SetActive(true);

        clockFormatTimer.SetTimer(/*building.BuildingStat.Level_Up_Time*/10);

        foreach (var currency in uiUpgradeCurrencies)
        {
            Destroy(currency.gameObject);
        }
        uiUpgradeCurrencies.Clear();
    }
}
