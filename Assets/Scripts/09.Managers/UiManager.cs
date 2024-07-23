using UnityEngine;
using UnityEngine.EventSystems;

public class UiManager : Singleton<UiManager>
{
    public UiCurrencies uiCurrencies;
    public UiMain mainUi;
    public UiAnimalFocus animalFocusUi;
    public UiSell sellUi;
    public UiFloorInformation floorInformationUi;
    public UiAnimalList animalListUi;
    public UiProducts productsUi;
    public UiCraftTable craftTableUi;
    public FloorMove floorMove;
    public StorageValueUi b4StorageValueUi;
    public StorageValueUi b5StorageValueUi;

    public bool isAnimalList = false;
    public bool isAnimalMove = false;

    private void Start()
    {
        ShowMainUi();
    }

    public void IsAnimalList(bool condition)
    {
        isAnimalList = condition;
        isAnimalMove = !condition;
    }

    public void IsAnimalMove(bool condition)
    {
        isAnimalList = !condition;
        isAnimalMove = condition;
    }

    public void OffAnimalList()
    {
        isAnimalList = false;
        isAnimalMove = false;
    }

    public void ShowCurrencyUi()
    {
        uiCurrencies.gameObject.SetActive(true);
        mainUi.gameObject.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.gameObject.SetActive(false);
        productsUi.gameObject.SetActive(false);
        craftTableUi.gameObject.SetActive(false);
        FloorManager.Instance.multiTouchOff = true;
        b4StorageValueUi.gameObject.SetActive(false);
        b5StorageValueUi.gameObject.SetActive(false);
    }

    public void ShowMainUi()
    {
        uiCurrencies.gameObject.SetActive(true);
        mainUi.gameObject.SetActive(true);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.gameObject.SetActive(false);
        productsUi.gameObject.SetActive(false);
        craftTableUi.gameObject.SetActive(false);
        FloorManager.Instance.multiTouchOff = false;
        b4StorageValueUi.gameObject.SetActive(false);
        b5StorageValueUi.gameObject.SetActive(false);
        OffAnimalList();
    }

    public void ShowAnimalFocusUi()
    {
        uiCurrencies.gameObject.SetActive(true);
        mainUi.gameObject.SetActive(false);
        animalFocusUi.gameObject.SetActive(true);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.gameObject.SetActive(false);
        productsUi.gameObject.SetActive(false);
        craftTableUi.gameObject.SetActive(false);
        FloorManager.Instance.multiTouchOff = true;
        b4StorageValueUi.gameObject.SetActive(false);
        b5StorageValueUi.gameObject.SetActive(false);
    }

    public void ShowSellUi()
    {
        uiCurrencies.gameObject.SetActive(true);
        mainUi.gameObject.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(true);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.gameObject.SetActive(false);
        productsUi.gameObject.SetActive(false);
        craftTableUi.gameObject.SetActive(false);
        FloorManager.Instance.multiTouchOff = true;
        b4StorageValueUi.gameObject.SetActive(false);
        b5StorageValueUi.gameObject.SetActive(false);
    }

    public void ShowFloorInformationUi()
    {
        uiCurrencies.gameObject.SetActive(true);
        mainUi.gameObject.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(true);
        animalListUi.gameObject.SetActive(false);
        productsUi.gameObject.SetActive(false);
        craftTableUi.gameObject.SetActive(false);
        FloorManager.Instance.multiTouchOff = true;
        b4StorageValueUi.gameObject.SetActive(false);
        b5StorageValueUi.gameObject.SetActive(false);
    }

    public void ShowAnimalListUi()
    {
        uiCurrencies.gameObject.SetActive(true);
        mainUi.gameObject.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.gameObject.SetActive(true);
        productsUi.gameObject.SetActive(false);
        craftTableUi.gameObject.SetActive(false);
        FloorManager.Instance.multiTouchOff = true;
        b4StorageValueUi.gameObject.SetActive(false);
        b5StorageValueUi.gameObject.SetActive(false);
    }

    public void ShowProductsUi()
    {
        uiCurrencies.gameObject.SetActive(true);
        mainUi.gameObject.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.gameObject.SetActive(false);
        productsUi.gameObject.SetActive(true);
        craftTableUi.gameObject.SetActive(false);
        FloorManager.Instance.multiTouchOff = true;
        b4StorageValueUi.gameObject.SetActive(false);
        b5StorageValueUi.gameObject.SetActive(false);
    }

    public void ShowCraftTableUi()
    {
        uiCurrencies.gameObject.SetActive(true);
        mainUi.gameObject.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.gameObject.SetActive(false);
        productsUi.gameObject.SetActive(false);
        craftTableUi.gameObject.SetActive(true);
        FloorManager.Instance.multiTouchOff = true;
        b4StorageValueUi.gameObject.SetActive(false);
        b5StorageValueUi.gameObject.SetActive(false);
    }

    public void SetProductCapacity(int capacity)
    {
        productsUi.SetCapacity(capacity);
        FloorManager.Instance.multiTouchOff = true;
    }

    public void SetSwipeDisable()
    {
        FloorManager.Instance.multiTouchOff = true;
    }
}
