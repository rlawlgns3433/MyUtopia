using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiFurnitureInfo : MonoBehaviour
{
    private static readonly string lvFormat = "Lv.{0}";
    public TextMeshProUGUI textFurnitureLevel;
    public Image furnitureProfile;
    public TextMeshProUGUI textFurnitureName;
    public TextMeshProUGUI textDescription;
    public Button buttonLevelUp;
    public Furniture furniture;
    public List<UiUpgradeCurrency> uiUpgradeCurrencies = new List<UiUpgradeCurrency>();
    public UiUpgradeCurrency uiUpgradeCurrency;
    public Transform contents; // 하위에 업그레이드 시 재화가 얼마나 필요한 지
    public bool Set(Furniture furniture)
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
            if (uiFurniture.furniture.FurnitureStat.FurnitureData.GetName().Equals(furniture.FurnitureStat.FurnitureData.GetName()))
                return false;
        }

        if (furniture.FurnitureStat.FurnitureData.Furniture_ID == 0)
            return false;

        this.furniture = furniture;

        buttonLevelUp.onClick.RemoveAllListeners();

        buttonLevelUp.onClick.AddListener(furniture.LevelUp);
        buttonLevelUp.onClick.AddListener(SetFurnitureUi);
        SetFurnitureUi();

        return true;
    }

    public async void SetFurnitureUi()
    {
        textFurnitureLevel.text = string.Format(lvFormat, furniture.FurnitureStat.Level);
        textFurnitureName.text = furniture.FurnitureStat.FurnitureData.GetName();

        if (furniture.FurnitureStat.Level_Up_Coin_Value != "0")
        {
            var currency = Instantiate(uiUpgradeCurrency, contents);
            var sprite = await DataTableMgr.GetResourceTable().Get((int)CurrencyType.Coin).GetImage();
            var value = furniture.FurnitureStat.Level_Up_Coin_Value.ToBigNumber();
            currency.SetCurrency(sprite, value);
            uiUpgradeCurrencies.Add(currency);
        }

        //uiBuildingInfo.buildingProfile.sprite = building.FurnitureData.GetProfile();
        //textDescription.text = 건물 설명
    }
}
