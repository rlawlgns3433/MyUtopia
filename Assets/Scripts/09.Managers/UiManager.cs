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
    public StorageValueUi b4StorageValueUi;
    public StorageValueUi b5StorageValueUi;
    public UiTutorial tutorialUi;
    public UiInvitation invitationUi;
    public TestPanel testPanelUi;

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
        invitationUi.gameObject.SetActive(false);
        tutorialUi.gameObject.SetActive(false);
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
        invitationUi.gameObject.SetActive(false);
        tutorialUi.gameObject.SetActive(false);
        testPanelUi.gameObject.SetActive(false);
        OffAnimalList();

        // 모든 동물의 말풍선 켜기
        foreach(var floor in FloorManager.Instance.floors.Values)
        {
            foreach(var animal in floor.animals)
            {
                if (animal.animalWork.canvasSpeech == null)
                    continue;
                animal.animalWork.canvasSpeech.gameObject.SetActive(true);
            }
        }

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
        b4StorageValueUi.gameObject.SetActive(false);
        b5StorageValueUi.gameObject.SetActive(false);
        invitationUi.gameObject.SetActive(false);
        tutorialUi.gameObject.SetActive(false);

        // 모든 동물의 말풍선 끄기
        foreach (var floor in FloorManager.Instance.floors.Values)
        {
            foreach (var animal in floor.animals)
            {
                if (animal.animalWork.canvasSpeech == null)
                    continue;
                animal.animalWork.canvasSpeech.gameObject.SetActive(false);
            }
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
        b4StorageValueUi.gameObject.SetActive(false);
        b5StorageValueUi.gameObject.SetActive(false);
        invitationUi.gameObject.SetActive(false);
        tutorialUi.gameObject.SetActive(false);
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
        invitationUi.gameObject.SetActive(false);
        tutorialUi.gameObject.SetActive(false);
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
        invitationUi.gameObject.SetActive(false);
        tutorialUi.gameObject.SetActive(false);
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
        invitationUi.gameObject.SetActive(false);
        tutorialUi.gameObject.SetActive(false);
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
        invitationUi.gameObject.SetActive(false);
        tutorialUi.gameObject.SetActive(false);
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
        b4StorageValueUi.gameObject.SetActive(false);
        b5StorageValueUi.gameObject.SetActive(false);
        invitationUi.gameObject.SetActive(true);
        tutorialUi.gameObject.SetActive(false);
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
        b4StorageValueUi.gameObject.SetActive(false);
        b5StorageValueUi.gameObject.SetActive(false);
        invitationUi.gameObject.SetActive(false);
        tutorialUi.gameObject.SetActive(true);
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
