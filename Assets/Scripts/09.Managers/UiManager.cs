using UnityEngine;

public class UiManager : Singleton<UiManager>
{
    public GameObject mainUi;
    public GameObject animalFocusUi;

    private void Start()
    {
        ShowMainUi();
    }

    public void ShowMainUi()
    {
        animalFocusUi.SetActive(false);
        mainUi.SetActive(true);
    }
    public void ShowAnimalFocusUi()
    {
        animalFocusUi.SetActive(true);
        mainUi.SetActive(false);
    }
}
