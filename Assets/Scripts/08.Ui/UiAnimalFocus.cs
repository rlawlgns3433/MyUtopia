using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UiAnimalFocus : MonoBehaviour
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
    [SerializeField]
    private Button buttonLevelUp;
    [SerializeField]
    private Button buttonSell;

    private AnimalWork currentAnimal;

    private void Start()
    {
        buttonLevelUp.onClick.AddListener(Set);
    }

    public void Set()
    {
        var animalWork = (ClickableManager.CurrentClicked as AnimalClick).AnimalWork;
        if(animalWork == null)
            return;
        currentAnimal = animalWork;

        Set(currentAnimal);
    }

    public void Set(AnimalWork animalWork)
    {
        var buttonText = buttonLevelUp.GetComponentInChildren<TextMeshProUGUI>();
        var animalData = animalWork.Animal.animalData;

        buttonText.text = animalData.Level == animalData.Level_Max ? mergeText : levelUpText;

        textAnimalName.text = animalData.GetName();
        textAnimalLevel.text = string.Format(levelFormat, animalData.Level, animalData.Level_Max);
        textCurrentFloor.text = string.Format(currentFloorFormat, animalWork.currentFloor);
        textAnimalWorkload.text = string.Format(workloadFormat, animalData.Workload);
    }
}
