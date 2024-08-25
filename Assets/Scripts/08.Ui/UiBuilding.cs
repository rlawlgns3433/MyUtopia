using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiBuildingInfo : MonoBehaviour, IUISetupable, IGrowable
{
    private static readonly string lvFormat = "Lv.{0}";
    public TextMeshProUGUI textBuildingLevel;
    public Image buildingProfile;
    public TextMeshProUGUI textBuildingName;
    public TextMeshProUGUI textDescription;
    public Image imageTextMax;
    public Image imageTextTimer;
    public Image imageDia;
    public Button buttonLevelUp;
    public Building building;
    public List<UiUpgradeCurrency> uiUpgradeCurrencies = new List<UiUpgradeCurrency>();
    public UiUpgradeCurrency uiBuildingUpgradeCurrency;
    public Transform contents; // ������ ���׷��̵� �� ��ȭ�� �󸶳� �ʿ��� ��
    public ClockFormatTimer clockFormatTimer;

    public bool IsUpgrading { get => building.BuildingStat.IsUpgrading; set => building.BuildingStat.IsUpgrading = value; }
    public double UpgradeTimeLeft
    {
        get => building.BuildingStat.UpgradeTimeLeft;
        set => building.BuildingStat.UpgradeTimeLeft = value;
    }
    public void FinishUpgrade()
    {
        SetDia();

        imageTextTimer.gameObject.SetActive(false);
        imageDia.gameObject.SetActive(false);
        if (FloorManager.Instance.touchManager.tutorial != null)
        {
            if (FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.CompleteLevelUp)
            {
                FloorManager.Instance.touchManager.tutorial.SetTutorialProgress();
            }
        }
    }

    public void LevelUp()
    {
        building.LevelUp();
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

        foreach (var uiBuilding in uiFloorInformation.uiBuildings)
        {
            if (uiBuilding.building.BuildingStat.BuildingData.GetName().Equals(building.BuildingStat.BuildingData.GetName()))
                return false;
        }

        if (building.BuildingStat.BuildingData.Building_ID == 0)
            return false;

        this.building = building;

        buttonLevelUp.onClick.RemoveAllListeners();
        buttonLevelUp.onClick.AddListener(SetStartUi);
        buttonLevelUp.onClick.AddListener(clockFormatTimer.StartClockTimer);
        buttonLevelUp.onClick.AddListener(SetDia);
        if (IsUpgrading)
        {
            if (clockFormatTimer.canStartTimer)
            {
                clockFormatTimer.SetTimer((int)building.BuildingStat.UpgradeTimeLeft);
                clockFormatTimer.StartClockTimer();
                clockFormatTimer.canStartTimer = false;
                IsUpgrading = true;
                foreach (var currency in uiUpgradeCurrencies)
                {
                    Destroy(currency.gameObject);
                }
                uiUpgradeCurrencies.Clear();
            }

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
        textBuildingLevel.text = string.Format(lvFormat, building.BuildingStat.Level);
        textBuildingName.text = building.BuildingStat.BuildingData.GetName();
        textDescription.text = building.BuildingStat.BuildingData.GetDescription();
        buildingProfile.sprite = await building.BuildingStat.BuildingData.GetProfile();

        buildingProfile.type = Image.Type.Sliced;
        buildingProfile.preserveAspect = true;

        if (IsUpgrading)
        {
            SetDia();
            imageDia.gameObject.SetActive(true);
            imageTextTimer.gameObject.SetActive(true);
            return;
        }
        else
        {
            imageTextTimer.gameObject.SetActive(false);
            imageDia.gameObject.SetActive(false);
        }

        foreach (var currency in uiUpgradeCurrencies)
        {
            Destroy(currency.gameObject);
        }
        uiUpgradeCurrencies.Clear();

        if (building.BuildingStat.Level_Up_Coin_Value != "0")
        {
            var currency = Instantiate(uiBuildingUpgradeCurrency, contents);
            uiUpgradeCurrencies.Add(currency);
            var sprite = await DataTableMgr.GetResourceTable().Get((int)CurrencyType.Coin).GetImage();
            var value = building.BuildingStat.Level_Up_Coin_Value.ToBigNumber();
            if (currency != null)
                currency.SetCurrency(sprite, value);
            else
                Destroy(currency.gameObject);
        }

        if (building.BuildingStat.Level_Up_Resource_1 != 0)
        {
            var currency = Instantiate(uiBuildingUpgradeCurrency, contents);
            uiUpgradeCurrencies.Add(currency);
            var sprite = await DataTableMgr.GetResourceTable().Get(building.BuildingStat.Level_Up_Resource_1).GetImage();
            var value = building.BuildingStat.Resource_1_Value.ToBigNumber();
            if (currency != null)
                currency.SetCurrency(sprite, value);
            else
                Destroy(currency.gameObject);
        }

        if (building.BuildingStat.Level_Up_Resource_2 != 0)
        {
            var currency = Instantiate(uiBuildingUpgradeCurrency, contents);
            uiUpgradeCurrencies.Add(currency);
            var sprite = await DataTableMgr.GetResourceTable().Get(building.BuildingStat.Level_Up_Resource_2).GetImage();
            var value = building.BuildingStat.Resource_2_Value.ToBigNumber();
            if (currency != null)
                currency.SetCurrency(sprite, value);
            else
                Destroy(currency.gameObject);
        }

        if (building.BuildingStat.Level_Up_Resource_3 != 0)
        {
            var currency = Instantiate(uiBuildingUpgradeCurrency, contents);
            uiUpgradeCurrencies.Add(currency);
            var sprite = await DataTableMgr.GetResourceTable().Get(building.BuildingStat.Level_Up_Resource_3).GetImage();
            var value = building.BuildingStat.Resource_3_Value.ToBigNumber();
            if (currency != null)
                currency.SetCurrency(sprite, value);
            else
                Destroy(currency.gameObject);
        }

        var floor = FloorManager.Instance.GetFloor($"B{building.BuildingStat.Floor_Type}");

        if (building.BuildingStat.Level == floor.FloorStat.Grade_Level_Max)
        {
            imageTextMax.gameObject.SetActive(true);
            foreach(var currency in uiUpgradeCurrencies)
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
        if (building.BuildingStat.IsUpgrading)
        {
            clockFormatTimer.canStartTimer = false;
            SetDia();
            UiManager.Instance.ShowConfirmPanelUi();
            UiManager.Instance.confirmPanelUi.SetText(CalculateDiamond(), this);
            return;
        }

        if(!CheckUpgradeCondition())
        {
            UiManager.Instance.warningPanelUi.SetWaring(WaringType.MaxLevel);
            UiManager.Instance.ShowWarningPanelUi();
            SoundManager.Instance.OnClickButton(SoundType.Caution);

            return;
        }

        if (!building.CheckCurrency())
        {
            clockFormatTimer.canStartTimer = false;
            UiManager.Instance.warningPanelUi.SetWaring(WaringType.OutOfMoney);
            UiManager.Instance.ShowWarningPanelUi();
            SoundManager.Instance.OnClickButton(SoundType.Caution);
            return;
        }

        SoundManager.Instance.OnClickButton(SoundType.LevelUpBuilding);

        clockFormatTimer.canStartTimer = true;
        IsUpgrading = true;
        building.SpendCurrency();

        imageTextTimer.gameObject.SetActive(true);
        imageDia.gameObject.SetActive(true);

        clockFormatTimer.SetTimer(building.BuildingStat.Level_Up_Time);

        foreach (var currency in uiUpgradeCurrencies)
        {
            Destroy(currency.gameObject);
        }

        uiUpgradeCurrencies.Clear();

        // Todo
        // Ÿ�̸� ���� ���� ���̾� �̹����� �Բ� �߰� �ʿ�

        SetDia();
    }
    public bool CheckUpgradeCondition()
    {
        var floor = FloorManager.Instance.GetFloor($"B{building.BuildingStat.Floor_Type}");

        if (floor == null)
            return false;

        if (building.BuildingStat.Level < floor.FloorStat.Grade_Level_Max)
            return true;

        return false;
    }

    // 1. �ʿ� ���̾� ���� ���
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