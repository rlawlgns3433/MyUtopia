using UnityEngine;

public class UiManager : Singleton<UiManager>
{
    public UiCurrencies uiCurrencies;
    public GameObject mainUi;
    public UiAnimalFocus animalFocusUi;
    public UiSell sellUi;
    public UiFloorInformation floorInformationUi;
    public GameObject animalListUi;
    public UiProducts productsUi;
    public UiCraftTable craftTableUi;
    public FloorMove floorMove;
    public StorageValueUi b4StorageValueUi;
    public StorageValueUi b5StorageValueUi;

    private void Start()
    {
        ShowMainUi();
    }
    public void ShowCurrencyUi()
    {
        uiCurrencies.gameObject.SetActive(true);
        mainUi.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.SetActive(false);
        productsUi.gameObject.SetActive(false);
        craftTableUi.gameObject.SetActive(false);
        floorMove.enabled = false;
        b4StorageValueUi.gameObject.SetActive(false);
        b5StorageValueUi.gameObject.SetActive(false);
    }

    public void ShowMainUi()
    {
        uiCurrencies.gameObject.SetActive(true);
        mainUi.SetActive(true);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.SetActive(false);
        productsUi.gameObject.SetActive(false);
        craftTableUi.gameObject.SetActive(false);
        floorMove.GetComponent<FloorMove>().enabled = true;
        b4StorageValueUi.gameObject.SetActive(false);
        b5StorageValueUi.gameObject.SetActive(false);
    }

    public void ShowAnimalFocusUi()
    {
        uiCurrencies.gameObject.SetActive(true);
        mainUi.SetActive(false);
        animalFocusUi.gameObject.SetActive(true);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.SetActive(false);
        productsUi.gameObject.SetActive(false);
        craftTableUi.gameObject.SetActive(false);
        floorMove.GetComponent<FloorMove>().enabled = false;
        b4StorageValueUi.gameObject.SetActive(false);
        b5StorageValueUi.gameObject.SetActive(false);
    }

    public void ShowSellUi()
    {
        uiCurrencies.gameObject.SetActive(true);
        mainUi.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(true);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.SetActive(false);
        productsUi.gameObject.SetActive(false);
        craftTableUi.gameObject.SetActive(false);
        floorMove.GetComponent<FloorMove>().enabled = false;
        b4StorageValueUi.gameObject.SetActive(false);
        b5StorageValueUi.gameObject.SetActive(false);
    }

    public void ShowFloorInformationUi()
    {
        uiCurrencies.gameObject.SetActive(true);
        mainUi.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(true);
        animalListUi.SetActive(false);
        productsUi.gameObject.SetActive(false);
        craftTableUi.gameObject.SetActive(false);
        floorMove.GetComponent<FloorMove>().enabled = false;
        b4StorageValueUi.gameObject.SetActive(false);
        b5StorageValueUi.gameObject.SetActive(false);
    }

    public void ShowAnimalListUi()
    {
        uiCurrencies.gameObject.SetActive(true);
        mainUi.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.SetActive(true);
        productsUi.gameObject.SetActive(false);
        craftTableUi.gameObject.SetActive(false);
        floorMove.GetComponent<FloorMove>().enabled = false;
        b4StorageValueUi.gameObject.SetActive(false);
        b5StorageValueUi.gameObject.SetActive(false);
    }

    public void ShowProductsUi()
    {
        uiCurrencies.gameObject.SetActive(true);
        mainUi.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.SetActive(false);
        productsUi.gameObject.SetActive(true);
        craftTableUi.gameObject.SetActive(false);
        floorMove.GetComponent<FloorMove>().enabled = false;
        b4StorageValueUi.gameObject.SetActive(false);
        b5StorageValueUi.gameObject.SetActive(false);
    }

    public void ShowCraftTableUi()
    {
        uiCurrencies.gameObject.SetActive(true);
        mainUi.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.SetActive(false);
        productsUi.gameObject.SetActive(false);
        craftTableUi.gameObject.SetActive(true);
        floorMove.GetComponent<FloorMove>().enabled = false;
        b4StorageValueUi.gameObject.SetActive(false);
        b5StorageValueUi.gameObject.SetActive(false);
    }

    public void SetProductCapacity(int capacity)
    {
        productsUi.SetCapacity(capacity);
        floorMove.GetComponent<FloorMove>().enabled = false;
    }

    public void SetSwipeDisable()
    {
        floorMove.GetComponent<FloorMove>().enabled = false;
    }
}
