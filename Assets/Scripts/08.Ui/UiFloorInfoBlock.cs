using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UiFloorInfoBlock : MonoBehaviour
{
    private static readonly string lvFormat = "Lv.{0} / Lv.{1}";
    public List<Image> imageProductions = new List<Image>();
    public TextMeshProUGUI textLevel;
    public UiUpgradeCurrency uiUpgradeCurrency;
    public Button buttonLevelUp;
    public Floor floor;
    public Transform contents;

    public bool Set(Floor floor)
    {
        var uiFloorInformation = UiManager.Instance.floorInformationUi;

        if (uiFloorInformation == null)
            return false;

        this.floor = floor;

        if (floor.FloorStat.FloorData.Floor_ID == 0)
            return false;

        buttonLevelUp.onClick.AddListener(floor.LevelUp);
        buttonLevelUp.onClick.AddListener(UiManager.Instance.floorInformationUi.SetFloorData);
        SetFloorUi();

        return true;
    }

    public async void SetFloorUi()
    {
        textLevel.text = string.Format(lvFormat, floor.FloorStat.Grade, floor.FloorStat.Grade_Max);

        for (int i = 0; i < floor.buildings.Count; i++)
        {
            var building = floor.buildings[i];
            if (building.isLock)
                continue;

            imageProductions[i].sprite = await building.BuildingStat.BuildingData.GetPrefab(); // 빌딩 이미지 추가 예정
            imageProductions[i].type = Image.Type.Sliced;
        }


        if (floor.FloorStat.Level_Up_Coin_Value != "0")
        {
            var currency = Instantiate(uiUpgradeCurrency, contents);
            var sprite = await DataTableMgr.GetResourceTable().Get((int)CurrencyType.Coin).GetImage();
            var value = floor.FloorStat.Level_Up_Coin_Value.ToBigNumber();
            currency.SetCurrency(sprite, value);
        }

        if (floor.FloorStat.Level_Up_Resource_1 != 0)
        {
            var currency = Instantiate(uiUpgradeCurrency, contents);
            var sprite = await DataTableMgr.GetResourceTable().Get(floor.FloorStat.Level_Up_Resource_1).GetImage();
            var value = floor.FloorStat.Resource_1_Value.ToBigNumber();
            currency.SetCurrency(sprite, value);
        }

        if (floor.FloorStat.Level_Up_Resource_2 != 0)
        {
            var currency = Instantiate(uiUpgradeCurrency, contents);
            var sprite = await DataTableMgr.GetResourceTable().Get(floor.FloorStat.Level_Up_Resource_2).GetImage();
            var value = floor.FloorStat.Resource_2_Value.ToBigNumber();
            currency.SetCurrency(sprite, value);
        }

        if (floor.FloorStat.Level_Up_Resource_3 != 0)
        {
            var currency = Instantiate(uiUpgradeCurrency, contents);
            var sprite = await DataTableMgr.GetResourceTable().Get(floor.FloorStat.Level_Up_Resource_3).GetImage();
            var value = floor.FloorStat.Resource_3_Value.ToBigNumber();
            currency.SetCurrency(sprite, value);
        }

        //textDescription.text = 건물 설명
    }
}
