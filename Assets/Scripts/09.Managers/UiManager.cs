using UnityEngine;

public class UiManager : Singleton<UiManager>
{
    public GameObject mainUi;
    public UiAnimalFocus animalFocusUi;

    private void Start()
    {
        ShowMainUi();
    }

    public void ShowMainUi()
    {
        animalFocusUi.gameObject.SetActive(false);
        mainUi.SetActive(true);
    }
    public void ShowAnimalFocusUi()
    {
        animalFocusUi.gameObject.SetActive(true);
        mainUi.SetActive(false);
    }
}
