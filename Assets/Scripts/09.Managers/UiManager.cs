using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    //public UiTutorial tutorialUi;
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
    public GameObject panelBlock;
    public GameObject catalougeImage;
    public Tutorial tutorial;
    private List<DTUiPanel> uiTweens = new List<DTUiPanel>();
    //public UiFocusOut focusOutUi;

    private void Awake()
    {
        uiTweens.Add(uiCurrencies.GetComponent<DTUiPanel>());
        uiTweens.Add(mainUi.GetComponent<DTUiPanel>());
        uiTweens.Add(animalFocusUi.GetComponent<DTUiPanel>());
        uiTweens.Add(sellUi.GetComponent<DTUiPanel>());
        uiTweens.Add(floorInformationUi.GetComponent<DTUiPanel>());
        uiTweens.Add(animalListUi.GetComponent<DTUiPanel>());
        uiTweens.Add(productsUi.GetComponent<DTUiPanel>());
        uiTweens.Add(craftTableUi.GetComponent<DTUiPanel>());
        uiTweens.Add(invitationUi.GetComponent<DTUiPanel>());
        uiTweens.Add(testPanelUi.GetComponent<DTUiPanel>());
        uiTweens.Add(patronBoardUi.GetComponent<DTUiPanel>());
        uiTweens.Add(uiMission.GetComponent<DTUiPanel>());
        uiTweens.Add(storageUi.GetComponent<DTUiPanel>());
        uiTweens.Add(uiCatalogue.GetComponent<DTUiPanel>());
        uiTweens.Add(currencyProductInventoryUi.GetComponent<DTUiPanel>());
         // 꺼진 오브젝트에 대해서 찾아와야함
    }
    private void Start()
    {
        DOTween.Init();

        if (PlayerPrefs.GetInt("TutorialCheck") == 1)
        {

            ShowStorageUi();
            tutorial.tutorialComplete = true;
        }
        else
        {
            ShowTutorial();
        }
            
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

    public void ShowTutorial()
    {
        if (tutorial != null)
            tutorial.tutorialComplete = false;
        tutorial.isStop = false;
        tutorial.gameObject.SetActive(true);
    }

    public void ShowCurrencyUi()
    {
        uiCurrencies.gameObject.GetComponent<DTUiPanel>().IsActive = true;

        //uiCurrencies.gameObject.SetActive(true);
        //mainUi.gameObject.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.gameObject.SetActive(false);
        productsUi.gameObject.SetActive(false);
        craftTableUi.gameObject.SetActive(false);
        FloorManager.Instance.multiTouchOff = true;
        invitationUi.gameObject.SetActive(false);
        patronBoardUi.gameObject.SetActive(false);
        uiMission.gameObject.SetActive(false);
        currencyProductInventoryUi.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
        uiCatalogue.gameObject.SetActive(false);
    }

    public void ShowStorageUi()
    {

        uiCurrencies.gameObject.GetComponent<DTUiPanel>().IsActive = true;

        //uiCurrencies.gameObject.SetActive(true);
        //mainUi.gameObject.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.gameObject.SetActive(false);
        productsUi.gameObject.SetActive(false);
        craftTableUi.gameObject.SetActive(false);
        FloorManager.Instance.multiTouchOff = true;
        invitationUi.gameObject.SetActive(false);
        patronBoardUi.gameObject.SetActive(false);
        uiMission.gameObject.SetActive(false);

        //storageUi.gameObject.SetActive(true);
        storageUi.gameObject.GetComponent<DTUiPanel>().IsActive = true;

        currencyProductInventoryUi.gameObject.SetActive(false);

        panel.gameObject.SetActive(true);

        uiCatalogue.gameObject.SetActive(false);
    }

    public void ShowMainUi()
    {

        foreach (var tween in uiTweens)
        {
            if (tween.IsActive && !(tween.panel == UiPanels.Main || tween.panel == UiPanels.Currencies))
            {
                tween.isFinishing = true;
            }

            if (tween.panel == UiPanels.Main || tween.panel == UiPanels.Currencies)
            {
                tween.IsActive = true;
            }
            else
            {
                tween.IsActive = false;
            }
        }
        //uiCurrencies.gameObject.SetActive(true);
        //mainUi.gameObject.SetActive(true);
        //animalFocusUi.gameObject.SetActive(false);
        //sellUi.gameObject.SetActive(false);
        //floorInformationUi.gameObject.SetActive(false);
        //animalListUi.gameObject.SetActive(false);
        //productsUi.gameObject.SetActive(false);
        //craftTableUi.gameObject.SetActive(false);
        if (!tutorial.gameObject.activeSelf)
        {
            FloorManager.Instance.multiTouchOff = false;
        }
        //invitationUi.gameObject.SetActive(false);
        //testPanelUi.gameObject.SetActive(false);
        //patronBoardUi.gameObject.SetActive(false);
        //uiMission.gameObject.SetActive(false);
        //storageUi.gameObject.SetActive(false);
        //currencyProductInventoryUi.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
        //uiCatalogue.gameObject.SetActive(false);
        OffAnimalList();
        if (FloorManager.Instance.touchManager.tutorial != null)
        {
            if(FloorManager.Instance.touchManager.tutorial.isClosing)
            {
                FloorManager.Instance.touchManager.tutorial.progress = TutorialProgress.None;
            } 
            if(FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.CloseItemUi || FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.CloseShop
                || FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.CloseAnimalStat || FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.CloseMurgeAnimalStat
                || FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.CloseAnimalList || FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.Clear)
            {
                FloorManager.Instance.touchManager.tutorial.SetTutorialProgress().Forget();
            }
        }
        animalListUi.mode = AnimalListMode.AnimalList;
    }

    public void ShowAnimalFocusUi()
    {
        uiCurrencies.gameObject.GetComponent<DTUiPanel>().IsActive = true;

        //uiCurrencies.gameObject.SetActive(true);
        mainUi.gameObject.SetActive(false);

        var focuseUi = animalFocusUi.gameObject.GetComponent<DTUiPanel>();
        if (FloorManager.Instance.touchManager.tutorial != null)
        {
            if (FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.ShowAnimalFocus)
            {
                FloorManager.Instance.touchManager.tutorial.SetTutorialProgress().Forget();
            }
        }
        focuseUi.IsActive = true;

        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.gameObject.SetActive(false);
        productsUi.gameObject.SetActive(false);
        craftTableUi.gameObject.SetActive(false);
        FloorManager.Instance.multiTouchOff = true;
        invitationUi.gameObject.SetActive(false);
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
        uiCurrencies.gameObject.GetComponent<DTUiPanel>().IsActive = true;

        //uiCurrencies.gameObject.SetActive(true);
        mainUi.gameObject.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        //sellUi.gameObject.SetActive(true);
        sellUi.gameObject.GetComponent<DTUiPanel>().IsActive = true;

        floorInformationUi.gameObject.SetActive(false);
        animalListUi.gameObject.SetActive(false);
        productsUi.gameObject.SetActive(false);
        craftTableUi.gameObject.SetActive(false);
        FloorManager.Instance.multiTouchOff = true;
        invitationUi.gameObject.SetActive(false);
        patronBoardUi.gameObject.SetActive(false);
        uiMission.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
        uiCatalogue.gameObject.SetActive(false);
        currencyProductInventoryUi.gameObject.SetActive(false);
    }

    public void ShowFloorInformationUi()
    {
        uiCurrencies.gameObject.GetComponent<DTUiPanel>().IsActive = true;
        //uiCurrencies.gameObject.SetActive(true);
        mainUi.gameObject.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        //floorInformationUi.gameObject.SetActive(true);
        if (FloorManager.Instance.touchManager.tutorial != null)
        {
            if (FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.FloorInfo)
            {
                FloorManager.Instance.touchManager.tutorial.SetTutorialProgress().Forget();
            }
        }
        floorInformationUi.gameObject.GetComponent<DTUiPanel>().IsActive = true;

        animalListUi.gameObject.SetActive(false);
        productsUi.gameObject.SetActive(false);
        craftTableUi.gameObject.SetActive(false);
        FloorManager.Instance.multiTouchOff = true;
        invitationUi.gameObject.SetActive(false);
        patronBoardUi.gameObject.SetActive(false);
        uiMission.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
        uiCatalogue.gameObject.SetActive(false);
        currencyProductInventoryUi.gameObject.SetActive(false);

    }

    public void ShowAnimalListUi()
    {
        uiCurrencies.gameObject.GetComponent<DTUiPanel>().IsActive = true;
        //uiCurrencies.gameObject.SetActive(true);
        mainUi.gameObject.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        if (FloorManager.Instance.touchManager.tutorial != null)
        {
            if (FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.AnimalList || FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.MoveAnimal)
            {
                FloorManager.Instance.touchManager.tutorial.SetTutorialProgress().Forget();
            }
        }
        animalListUi.gameObject.GetComponent<DTUiPanel>().IsActive = true;
        //animalListUi.gameObject.SetActive(true);
        productsUi.gameObject.SetActive(false);
        craftTableUi.gameObject.SetActive(false);
        FloorManager.Instance.multiTouchOff = true;
        invitationUi.gameObject.SetActive(false);
        patronBoardUi.gameObject.SetActive(false);
        uiMission.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
        uiCatalogue.gameObject.SetActive(false);
        currencyProductInventoryUi.gameObject.SetActive(false);

    }

    public void ShowProductsUi()
    {
        uiCurrencies.gameObject.GetComponent<DTUiPanel>().IsActive = true;
        //uiCurrencies.gameObject.SetActive(true);
        mainUi.gameObject.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.gameObject.SetActive(false);

        productsUi.gameObject.GetComponent<DTUiPanel>().IsActive = true;
        //productsUi.gameObject.SetActive(true);

        craftTableUi.gameObject.SetActive(false);
        FloorManager.Instance.multiTouchOff = true;
        invitationUi.gameObject.SetActive(false);
        patronBoardUi.gameObject.SetActive(false);
        uiMission.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
        uiCatalogue.gameObject.SetActive(false);
        currencyProductInventoryUi.gameObject.SetActive(false);
        if (FloorManager.Instance.touchManager.tutorial != null)
        {
            if (FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.OpenShop)
            {
                FloorManager.Instance.touchManager.tutorial.progress = TutorialProgress.None;
            }
        }
    }

    public void ShowSettingUi()
    {
        uiCurrencies.gameObject.GetComponent<DTUiPanel>().IsActive = true;

        //uiCurrencies.gameObject.SetActive(true);
        mainUi.gameObject.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.gameObject.SetActive(false);
        productsUi.gameObject.SetActive(false);
        craftTableUi.gameObject.SetActive(false);
        FloorManager.Instance.multiTouchOff = true;

        testPanelUi.gameObject.GetComponent<DTUiPanel>().IsActive = true;
        invitationUi.gameObject.SetActive(false);

        patronBoardUi.gameObject.SetActive(false);
        uiMission.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
        uiCatalogue.gameObject.SetActive(false);
        currencyProductInventoryUi.gameObject.SetActive(false);
    }

    public void ShowCraftTableUi()
    {
        uiCurrencies.gameObject.GetComponent<DTUiPanel>().IsActive = true;
        //uiCurrencies.gameObject.SetActive(true);
        mainUi.gameObject.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.gameObject.SetActive(false);
        productsUi.gameObject.SetActive(false);
        if (FloorManager.Instance.touchManager.tutorial != null)
        {
            if (FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.OpenCraftTable)
            {
                FloorManager.Instance.touchManager.tutorial.SetTutorialProgress().Forget();
            }
        }
        craftTableUi.gameObject.GetComponent<DTUiPanel>().IsActive = true;
        //craftTableUi.gameObject.SetActive(true);

        FloorManager.Instance.multiTouchOff = true;
        invitationUi.gameObject.SetActive(false);
        patronBoardUi.gameObject.SetActive(false);
        uiMission.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
        uiCatalogue.gameObject.SetActive(false);
        currencyProductInventoryUi.gameObject.SetActive(false);

    }

    public void ShowInvitationUi()
    {
        uiCurrencies.gameObject.GetComponent<DTUiPanel>().IsActive = true;


        //uiCurrencies.gameObject.SetActive(true);
        mainUi.gameObject.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.gameObject.SetActive(false);
        productsUi.gameObject.SetActive(false);
        craftTableUi.gameObject.SetActive(false);
        FloorManager.Instance.multiTouchOff = true;

        invitationUi.gameObject.GetComponent<DTUiPanel>().IsActive = true;
        //invitationUi.gameObject.SetActive(true);
        if (FloorManager.Instance.touchManager.tutorial != null)
        {
            if (FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.PurchaseAnimal || FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.MurgeAnimalPurchase)
            {
                FloorManager.Instance.touchManager.tutorial.SetTutorialProgress();
            }
        }
        patronBoardUi.gameObject.SetActive(false);
        uiMission.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
        uiCatalogue.gameObject.SetActive(false);
        currencyProductInventoryUi.gameObject.SetActive(false);
    }

    public void ShowTutorialUi()
    {


        uiCurrencies.gameObject.GetComponent<DTUiPanel>().IsActive = true;
        //uiCurrencies.gameObject.SetActive(true);
        mainUi.gameObject.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.gameObject.SetActive(false);
        productsUi.gameObject.SetActive(false);
        craftTableUi.gameObject.SetActive(false);
        FloorManager.Instance.multiTouchOff = true;
        invitationUi.gameObject.SetActive(false);
        testPanelUi.gameObject.SetActive(false);
        patronBoardUi.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
        uiCatalogue.gameObject.SetActive(false);
        currencyProductInventoryUi.gameObject.SetActive(false);
    }

    public void ShowPatronUi()
    {


        uiCurrencies.gameObject.GetComponent<DTUiPanel>().IsActive = true;
        //uiCurrencies.gameObject.SetActive(true);
        mainUi.gameObject.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.gameObject.SetActive(false);
        productsUi.gameObject.SetActive(false);
        craftTableUi.gameObject.SetActive(false);
        FloorManager.Instance.multiTouchOff = true;
        invitationUi.gameObject.SetActive(false);
        testPanelUi.gameObject.SetActive(false);

        patronBoardUi.gameObject.GetComponent<DTUiPanel>().IsActive = true;
        //patronBoardUi.gameObject.SetActive(true);

        uiMission.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
        uiCatalogue.gameObject.SetActive(false);
        currencyProductInventoryUi.gameObject.SetActive(false);
    }

    public void ShowMissionUi()
    {


        uiCurrencies.gameObject.GetComponent<DTUiPanel>().IsActive = true;
        //uiCurrencies.gameObject.SetActive(true);
        mainUi.gameObject.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.gameObject.SetActive(false);
        productsUi.gameObject.SetActive(false);
        craftTableUi.gameObject.SetActive(false);
        FloorManager.Instance.multiTouchOff = true;
        invitationUi.gameObject.SetActive(false);
        testPanelUi.gameObject.SetActive(false);
        patronBoardUi.gameObject.SetActive(false);

        uiMission.gameObject.GetComponent<DTUiPanel>().IsActive = true;
        //uiMission.gameObject.SetActive(true);

        currencyProductInventoryUi.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
        uiCatalogue.gameObject.SetActive(false);
    }
    public void ShowCatalougeUi()
    {


        uiCurrencies.gameObject.GetComponent<DTUiPanel>().IsActive = true;
        //uiCurrencies.gameObject.SetActive(true);
        mainUi.gameObject.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.gameObject.SetActive(false);
        productsUi.gameObject.SetActive(false);
        craftTableUi.gameObject.SetActive(false);
        FloorManager.Instance.multiTouchOff = true;
        invitationUi.gameObject.SetActive(false);
        testPanelUi.gameObject.SetActive(false);
        patronBoardUi.gameObject.SetActive(false);
        uiMission.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
        uiCatalogue.gameObject.GetComponent<DTUiPanel>().IsActive = true;
        //uiCatalogue.gameObject.SetActive(true);
    }

    public void ShowCurrencyProductInventoryUi()
    {


        uiCurrencies.gameObject.GetComponent<DTUiPanel>().IsActive = true;
        //uiCurrencies.gameObject.SetActive(true);
        mainUi.gameObject.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.gameObject.SetActive(false);
        productsUi.gameObject.SetActive(false);
        craftTableUi.gameObject.SetActive(false);
        FloorManager.Instance.multiTouchOff = true;
        invitationUi.gameObject.SetActive(false);
        testPanelUi.gameObject.SetActive(false);
        patronBoardUi.gameObject.SetActive(false);
        uiMission.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);

        if (FloorManager.Instance.touchManager.tutorial != null)
        {
            if(FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.Product)
            {
                FloorManager.Instance.touchManager.tutorial.SetTutorialProgress().Forget();
            }
        }
        currencyProductInventoryUi.gameObject.GetComponent<DTUiPanel>().IsActive = true;
        //currencyProductInventoryUi.gameObject.SetActive(true);

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
        if (tutorial.gameObject.activeSelf)

        catalougeImage.SetActive(value);
    }

    //public void SetFocusOut()
    //{
    //    focusOutUi.FocusOut();
    //}
}
