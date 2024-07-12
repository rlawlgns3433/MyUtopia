using UnityEngine;

public class UiManager : Singleton<UiManager>
{
    public GameObject mainUi;
    public UiAnimalFocus animalFocusUi;
    public UiSell uiSell;
    public UiFloorInformation uiFloorInformation;

    private void Start()
    {
        ShowMainUi();
    }

    public void ShowMainUi()
    {
        animalFocusUi.gameObject.SetActive(false);
        mainUi.SetActive(true);
        uiSell.gameObject.SetActive(false);
        uiFloorInformation.gameObject.SetActive(false);
    }
    public void ShowAnimalFocusUi()
    {
        animalFocusUi.gameObject.SetActive(true);
        mainUi.SetActive(false);
        uiSell.gameObject.SetActive(false);
        uiFloorInformation.gameObject.SetActive(false);
    }

    public void ShowSellUi()
    {
        animalFocusUi.gameObject.SetActive(false);
        mainUi.SetActive(false);
        uiSell.gameObject.SetActive(true);
        uiFloorInformation.gameObject.SetActive(false);
    }

    public void ShowFloorInformationUi()
    {
        animalFocusUi.gameObject.SetActive(false);
        mainUi.SetActive(false);
        uiSell.gameObject.SetActive(false);
        uiFloorInformation.gameObject.SetActive(true);
    }
}
