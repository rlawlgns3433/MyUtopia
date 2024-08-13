using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Composites;
using UnityEngine.UI;

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
    public UiTutorial tutorialUi;
    public UiInvitation invitationUi;
    public TestPanel testPanelUi;
    public UiPatronBoard patronBoardUi;
    public UiMissionList uiMission;
    public StorageUi storageUi;
    public UiCatalogue uiCatalogue;
    public UiCurrencyProductInventory currencyProductInventoryUi;
    public GameObject bottom;
    public Button changeButton;
    public bool isAnimalList = false;
    public bool isAnimalMove = false;
    public GameObject panel;
    public GameObject catalougeImage;
    private void Start()
    {
        if (PlayerPrefs.GetInt("TutorialCheck") == 1)
            ShowStorageUi();
            //ShowMainUi();
        else
            ShowTutorialUi();
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
        invitationUi.gameObject.SetActive(false);
        tutorialUi.gameObject.SetActive(false);
        patronBoardUi.gameObject.SetActive(false);
        uiMission.gameObject.SetActive(false);
        currencyProductInventoryUi.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
        uiCatalogue.gameObject.SetActive(false);
    }

    public void ShowStorageUi()
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
        invitationUi.gameObject.SetActive(false);
        tutorialUi.gameObject.SetActive(false);
        patronBoardUi.gameObject.SetActive(false);
        uiMission.gameObject.SetActive(false);
        storageUi.gameObject.SetActive(true);
        currencyProductInventoryUi.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
        uiCatalogue.gameObject.SetActive(false);
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
        invitationUi.gameObject.SetActive(false);
        tutorialUi.gameObject.SetActive(false);
        testPanelUi.gameObject.SetActive(false);
        patronBoardUi.gameObject.SetActive(false);
        uiMission.gameObject.SetActive(false);
        storageUi.gameObject.SetActive(false);
        currencyProductInventoryUi.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
        uiCatalogue.gameObject.SetActive(false);
        OffAnimalList();

        animalListUi.mode = AnimalListMode.AnimalList;
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
        invitationUi.gameObject.SetActive(false);
        tutorialUi.gameObject.SetActive(false);
        patronBoardUi.gameObject.SetActive(false);
        uiMission.gameObject.SetActive(false);
        uiCatalogue.gameObject.SetActive(false);
        currencyProductInventoryUi.gameObject.SetActive(false);
        if(sellUi.gameObject.activeSelf)
        {
            uiMission.gameObject.SetActive(true);
        }
        else
        {
            panel.gameObject.SetActive(false);
        }
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
        invitationUi.gameObject.SetActive(false);
        tutorialUi.gameObject.SetActive(false);
        patronBoardUi.gameObject.SetActive(false);
        uiMission.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
        uiCatalogue.gameObject.SetActive(false);
        currencyProductInventoryUi.gameObject.SetActive(false);
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
        invitationUi.gameObject.SetActive(false);
        tutorialUi.gameObject.SetActive(false);
        patronBoardUi.gameObject.SetActive(false);
        uiMission.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
        uiCatalogue.gameObject.SetActive(false);
        currencyProductInventoryUi.gameObject.SetActive(false);
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
        invitationUi.gameObject.SetActive(false);
        tutorialUi.gameObject.SetActive(false);
        patronBoardUi.gameObject.SetActive(false);
        uiMission.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
        uiCatalogue.gameObject.SetActive(false);
        currencyProductInventoryUi.gameObject.SetActive(false);
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
        invitationUi.gameObject.SetActive(false);
        tutorialUi.gameObject.SetActive(false);
        patronBoardUi.gameObject.SetActive(false);
        uiMission.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
        uiCatalogue.gameObject.SetActive(false);
        currencyProductInventoryUi.gameObject.SetActive(false);
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
        invitationUi.gameObject.SetActive(false);
        tutorialUi.gameObject.SetActive(false);
        patronBoardUi.gameObject.SetActive(false);
        uiMission.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
        uiCatalogue.gameObject.SetActive(false);
        currencyProductInventoryUi.gameObject.SetActive(false);
    }

    public void ShowInvitationUi()
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
        invitationUi.gameObject.SetActive(true);
        tutorialUi.gameObject.SetActive(false);
        patronBoardUi.gameObject.SetActive(false);
        uiMission.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
        uiCatalogue.gameObject.SetActive(false);
        currencyProductInventoryUi.gameObject.SetActive(false);
    }

    public void ShowTutorialUi()
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
        invitationUi.gameObject.SetActive(false);
        tutorialUi.gameObject.SetActive(true);
        testPanelUi.gameObject.SetActive(false);
        patronBoardUi.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
        uiCatalogue.gameObject.SetActive(false);
        currencyProductInventoryUi.gameObject.SetActive(false);
    }

    public void ShowPatronUi()
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
        invitationUi.gameObject.SetActive(false);
        tutorialUi.gameObject.SetActive(false);
        testPanelUi.gameObject.SetActive(false);
        patronBoardUi.gameObject.SetActive(true);
        uiMission.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
        uiCatalogue.gameObject.SetActive(false);
        currencyProductInventoryUi.gameObject.SetActive(false);
    }

    public void ShowMissionUi()
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
        invitationUi.gameObject.SetActive(false);
        tutorialUi.gameObject.SetActive(false);
        testPanelUi.gameObject.SetActive(false);
        patronBoardUi.gameObject.SetActive(false);
        uiMission.gameObject.SetActive(true);
        currencyProductInventoryUi.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
        uiCatalogue.gameObject.SetActive(false);
    }
    public void ShowCatalougeUi()
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
        invitationUi.gameObject.SetActive(false);
        tutorialUi.gameObject.SetActive(false);
        testPanelUi.gameObject.SetActive(false);
        patronBoardUi.gameObject.SetActive(false);
        uiMission.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
        uiCatalogue.gameObject.SetActive(true);
    }

    public void ShowCurrencyProductInventoryUi()
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
        invitationUi.gameObject.SetActive(false);
        tutorialUi.gameObject.SetActive(false);
        testPanelUi.gameObject.SetActive(false);
        patronBoardUi.gameObject.SetActive(false);
        uiMission.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
        currencyProductInventoryUi.gameObject.SetActive(true);
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
    public void SetSwipeEnable()
    {
        FloorManager.Instance.multiTouchOff = false;
    }

    public void CoverBottom()
    {
        if(bottom.gameObject.activeSelf)
        {
            bottom.gameObject.SetActive(false);
            var rotation = changeButton.gameObject.transform.rotation;
            rotation.z = 180;
            changeButton.gameObject.transform.rotation = rotation;
        }
        else
        {
            bottom.gameObject.SetActive(true);
            var rotation = changeButton.gameObject.transform.rotation;
            rotation.z = 0;
            changeButton.gameObject.transform.rotation = rotation;
        }
    }

    public void SetCatalougeImage(bool value)
    {
        catalougeImage.SetActive(value);
    }
}
