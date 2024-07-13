using UnityEngine;

public class UiManager : Singleton<UiManager>
{
    public GameObject currencyUi;
    public GameObject mainUi;
    public UiAnimalFocus animalFocusUi;
    public UiSell sellUi;
    public UiFloorInformation floorInformationUi;
    public GameObject animalListUi;

    private void Start()
    {
        ShowMainUi();
    }
    public void ShowCurrencyUi()
    {
        currencyUi.SetActive(true);
        mainUi.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.SetActive(false);
    }

    public void ShowMainUi()
    {
        currencyUi.SetActive(true);
        mainUi.SetActive(true);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.SetActive(false);
    }

    public void ShowAnimalFocusUi()
    {
        currencyUi.SetActive(true);
        mainUi.SetActive(false);
        animalFocusUi.gameObject.SetActive(true);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.SetActive(false);
    }

    public void ShowSellUi()
    {
        currencyUi.SetActive(false);
        mainUi.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(true);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.SetActive(false);
    }

    public void ShowFloorInformationUi()
    {
        currencyUi.SetActive(false);
        mainUi.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(true);
        animalListUi.SetActive(false);
    }

    public void ShowAnimalListUi()
    {
        currencyUi.SetActive(false);
        mainUi.SetActive(false);
        animalFocusUi.gameObject.SetActive(false);
        sellUi.gameObject.SetActive(false);
        floorInformationUi.gameObject.SetActive(false);
        animalListUi.SetActive(true);
    }
}
