using Cysharp.Threading.Tasks;
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
    public Image imageTextMax;
    public Image imageTextTimer;
    public Image imageDia;
    public TextMeshProUGUI textFloorDesc;
    public UiUpgradeCurrency uiUpgradeCurrency;
    public List<UiUpgradeCurrency> uiUpgradeCurrencies = new List<UiUpgradeCurrency>();
    public Button buttonLevelUp;
    public Floor floor;
    public Transform contents;
    public ClockFormatTimer clockFormatTimer;

    public bool IsUpgrading 
    { 
        get => floor.FloorStat.IsUpgrading;
        set => floor.FloorStat.IsUpgrading = value;
    }

    //private void Start()
    //{
    //    SetTimerWhenStartUp();
    //}

    //private void OnDisable()
    //{
    //    floor.FloorStat.UpgradeTimeLeft = Mathf.FloorToInt(clockFormatTimer.remainingTime);
    //    floor.disabledTime = Time.time;
    //}

    //private void SetTimerWhenStartUp()
    //{
    //    if (!IsUpgrading)
    //        return;
    //    clockFormatTimer.canStartTimer = true;
    //    imageDia.gameObject.SetActive(true);
    //    imageTextTimer.gameObject.SetActive(true);

    //    if(floor.FloorStat.UpgradeTimeLeft < 1)
    //    {
    //        floor.FloorStat.UpgradeTimeLeft = 1;
    //    }

    //    clockFormatTimer.SetTimer(floor.FloorStat.UpgradeTimeLeft/* - Mathf.FloorToInt(DateTime.UtcNow.Hour * 3600 + DateTime.UtcNow.Minute * 60 + DateTime.UtcNow.Second - floor.FloorStat.UpgradeStartTime)*/);

    //    foreach (var currency in uiUpgradeCurrencies)
    //    {
    //        Destroy(currency.gameObject);
    //    }
    //    uiUpgradeCurrencies.Clear();
    //    imageTextMax.gameObject.SetActive(false);
    //    clockFormatTimer.StartClockTimer();
    //}

    public void FinishUpgrade()
    {
        imageDia.gameObject.SetActive(false);
        imageTextTimer.gameObject.SetActive(false);
    }

    public void LevelUp()
    {
        floor.LevelUp();
    }

    public bool Set(Floor floor)
    {
        foreach (var currency in uiUpgradeCurrencies)
        {
            Destroy(currency.gameObject);
        }
        uiUpgradeCurrencies.Clear();

        var uiFloorInformation = UiManager.Instance.floorInformationUi;

        if (uiFloorInformation == null)
            return false;


        if (floor.FloorStat.FloorData.Floor_ID == 0)
            return false;

        this.floor = floor;

        buttonLevelUp.onClick.RemoveAllListeners();
        buttonLevelUp.onClick.AddListener(SetStartUi);
        buttonLevelUp.onClick.AddListener(clockFormatTimer.StartClockTimer);
        buttonLevelUp.onClick.AddListener(SetDia);

        if (IsUpgrading)
        {
            SetDia();
            imageTextTimer.gameObject.SetActive(true);
            imageDia.gameObject.SetActive(true);
        }
        else
        {
            SetFinishUi();
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

        if (IsUpgrading)
        {
            SetDia();
            imageDia.gameObject.SetActive(true);
            imageTextTimer.gameObject.SetActive(true);
            return;
        }
        else
        {
            imageDia.gameObject.SetActive(false);
            imageTextTimer.gameObject.SetActive(false);
        }

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
            uiUpgradeCurrencies.Add(currency);
            var sprite = await DataTableMgr.GetResourceTable().Get((int)CurrencyType.Coin).GetImage();
            var value = floor.FloorStat.Level_Up_Coin_Value.ToBigNumber();
            if (currency != null)
                currency.SetCurrency(sprite, value);
            else
                Destroy(currency.gameObject);
        }

        if (floor.FloorStat.Level_Up_Resource_1 != 0)
        {
            var currency = Instantiate(uiUpgradeCurrency, contents);
            uiUpgradeCurrencies.Add(currency);
            var sprite = await DataTableMgr.GetResourceTable().Get(floor.FloorStat.Level_Up_Resource_1).GetImage();
            var value = floor.FloorStat.Resource_1_Value.ToBigNumber();
            if (currency != null)
                currency.SetCurrency(sprite, value);
            else
                Destroy(currency.gameObject);
        }

        if (floor.FloorStat.Level_Up_Resource_2 != 0)
        {
            var currency = Instantiate(uiUpgradeCurrency, contents);
            uiUpgradeCurrencies.Add(currency);
            var sprite = await DataTableMgr.GetResourceTable().Get(floor.FloorStat.Level_Up_Resource_2).GetImage();
            var value = floor.FloorStat.Resource_2_Value.ToBigNumber();
            if (currency != null)
                currency.SetCurrency(sprite, value);
            else
                Destroy(currency.gameObject);
        }

        if (floor.FloorStat.Level_Up_Resource_3 != 0)
        {
            var currency = Instantiate(uiUpgradeCurrency, contents);
            uiUpgradeCurrencies.Add(currency);
            var sprite = await DataTableMgr.GetResourceTable().Get(floor.FloorStat.Level_Up_Resource_3).GetImage();
            var value = floor.FloorStat.Resource_3_Value.ToBigNumber();
            if (currency != null)
                currency.SetCurrency(sprite, value);
            else
                Destroy(currency.gameObject);
        }


        if (floor.FloorStat.Grade == floor.FloorStat.Grade_Max)
        {
            imageTextMax.gameObject.SetActive(true);
            foreach (var currency in uiUpgradeCurrencies)
            {
                currency.gameObject.SetActive(false);
            }
            return;
        }
        else
        {
            imageTextMax.gameObject.SetActive(false);

            foreach (var currency in uiUpgradeCurrencies)
            {
                currency.gameObject.SetActive(true);
            }
        }
    }

    public void SetStartUi()
    {
        clockFormatTimer.canStartTimer = false;

        if (floor.FloorStat.IsUpgrading)
        {
            // Todo
            // 클릭 시 다이아 소모하면서 시간 단축
            // 1. 필요 다이아 개수 계산
            // 2. 필요 다이아를 소모할 것인지 확인
            // 3. 필요 다이아를 소모해서 시간 단축과 함께 업그레이드 완료
            SetDia();
            UiManager.Instance.ShowConfirmPanelUi();
            UiManager.Instance.confirmPanelUi.SetText(CalculateDiamond(), this);
            return;
        }

        if (floor.FloorStat.Grade == floor.FloorStat.Grade_Max)
        {
            UiManager.Instance.warningPanelUi.SetWaring(WaringType.MaxLevel);
            UiManager.Instance.ShowWarningPanelUi();
            SoundManager.Instance.OnClickButton(SoundType.Caution);
            return;
        }


        if (!CheckUpgradeCondition())
        {
            // 모든 건물이 레벨을 충족하지 못함
            UiManager.Instance.ShowWarningPanelUi();
            UiManager.Instance.warningPanelUi.SetWaring(WaringType.FloorUpgrade);
            SoundManager.Instance.OnClickButton(SoundType.Caution);
            return;
        }

        if (!floor.CheckCurrency())
        {
            UiManager.Instance.warningPanelUi.SetWaring(WaringType.OutOfMoney);
            UiManager.Instance.ShowWarningPanelUi();
            SoundManager.Instance.OnClickButton(SoundType.Caution);
            return;
        }

        SoundManager.Instance.OnClickButton(SoundType.LevelUpBuilding);

        clockFormatTimer.canStartTimer = true;
        IsUpgrading = true;
        floor.SpendCurrency();

        imageTextTimer.gameObject.SetActive(true);
        imageDia.gameObject.SetActive(true);
        clockFormatTimer.SetTimer(floor.FloorStat.Level_Up_Time);

        foreach (var currency in uiUpgradeCurrencies)
        {
            Destroy(currency.gameObject);
        }
        uiUpgradeCurrencies.Clear();

        // Todo
        // 타이머 설정 이후 다이아 이미지와 함께 추가 필요
        SetDia();
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

    public BigNumber CalculateDiamond()
    {
        if (clockFormatTimer.remainingTime <= 0)
        {
            UniTask.WaitForSeconds(0.02f);
            return new BigNumber(Mathf.CeilToInt(clockFormatTimer.timerDuration / 30));
        }

        return new BigNumber(Mathf.CeilToInt(clockFormatTimer.remainingTime / 30));
    }

    public void SetDia()
    {
        var diamondReCalc = CalculateDiamond();
        imageDia.GetComponentInChildren<UiUpgradeCurrency>().SetCurrency(diamondReCalc);
    }
}
