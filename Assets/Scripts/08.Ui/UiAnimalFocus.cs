using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class UiAnimalFocus : Observer
{
    private static readonly string levelFormat = "Level : {0} / {1}";
    private static readonly string workloadFormat = "Workload : {0} / s"; // xxx / s
    private static readonly string currentFloorFormat = "Floor : {0}"; // xxx / s
    private static readonly string levelUpText = "LevelUp"; 
    private static readonly string mergeText = "Merge";

    [SerializeField]
    private Image imageAnimalPotrait;
    [SerializeField]
    private Slider sliderAnimalStamina;
    [SerializeField]
    private TextMeshProUGUI textAnimalName;
    [SerializeField]
    private TextMeshProUGUI textAnimalLevel;
    [SerializeField]
    private TextMeshProUGUI textCurrentFloor;
    [SerializeField]
    private TextMeshProUGUI textAnimalWorkload;
    public Button buttonLevelUp;
    public Button buttonSell;

    private AnimalWork currentAnimal;

    private void Start()
    {
        buttonLevelUp.onClick.AddListener(Set);
        buttonSell.onClick.AddListener(SetSaleUi);
    }

    public void Set()
    {
        var animalClick = ClickableManager.CurrentClicked as AnimalClick;
        if(animalClick == null)
        {
            UiManager.Instance.animalFocusUi.buttonLevelUp.interactable = false;
            UiManager.Instance.animalFocusUi.buttonSell.interactable = false;
            return;
        }

        var animalWork = animalClick.AnimalWork;

        if(animalWork == null)
        {
            UiManager.Instance.animalFocusUi.buttonLevelUp.interactable = false;
            UiManager.Instance.animalFocusUi.buttonSell.interactable = false;
            return;
        }
        UiManager.Instance.animalFocusUi.buttonLevelUp.interactable = true;
        UiManager.Instance.animalFocusUi.buttonSell.interactable = true;
        currentAnimal = animalWork;

        Set(currentAnimal);
    }

    public async void Set(AnimalWork animalWork)
    {
        currentAnimal.DetachObserver(this);
        currentAnimal.AttachObserver(this);

        sliderAnimalStamina.minValue = 0;
        sliderAnimalStamina.maxValue = animalWork.Animal.animalStat.AnimalData.Stamina;
        sliderAnimalStamina.value = animalWork.Animal.animalStat.Stamina;

        imageAnimalPotrait.sprite = await animalWork.Animal.animalStat.AnimalData.GetProfile();

        var buttonText = buttonLevelUp.GetComponentInChildren<TextMeshProUGUI>();
        var animalData = animalWork.Animal.animalStat.AnimalData;

        buttonText.text = animalData.Level == animalData.Level_Max ? mergeText : levelUpText;

        textAnimalName.text = animalData.GetName();
        textAnimalLevel.text = string.Format(levelFormat, animalData.Level, animalData.Level_Max);
        textCurrentFloor.text = string.Format(currentFloorFormat, animalWork.Animal.animalStat.CurrentFloor);
        textAnimalWorkload.text = string.Format(workloadFormat, animalData.Workload.ToBigNumber().ToString());
    }

    public void SetSaleUi()
    {
        UiManager.Instance.sellUi.textCoinForSale.text = currentAnimal.Animal.animalStat.Sale_Coin.ToBigNumber().ToString();
    }

    public override void Notify(Subject subject)
    {
        sliderAnimalStamina.value = currentAnimal.Animal.animalStat.Stamina;
    }
}
