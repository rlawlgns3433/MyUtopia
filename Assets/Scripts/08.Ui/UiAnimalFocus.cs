using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UiAnimalFocus : Observer
{
    private static readonly string levelFormat = "레벨 : {0} / {1}";
    private static readonly string workloadFormat = "업무량 : {0} / s"; // xxx / s
    private static readonly string currentFloorFormat = "현재층 : {0}"; //
    private static readonly string levelUpText = "레벨업"; 
    private static readonly string mergeText = "머지";

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
    private TextMeshProUGUI textLevelUpCost;
    [SerializeField]
    private TextMeshProUGUI textSellCost;
    public Button buttonLevelUp;
    public Button buttonSell;

    private AnimalWork currentAnimal;
    public GameObject levelUpCostGo;

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

        if(animalData.Level == animalData.Level_Max)
        {
            buttonText.text = mergeText;

            if(animalWork.Animal.CanMerge(out var target))
            {
                UiManager.Instance.animalFocusUi.buttonLevelUp.interactable = true;
            }
            else
            {
                UiManager.Instance.animalFocusUi.buttonLevelUp.interactable = false;
            }
        }
        else
        {
            buttonText.text = levelUpText;
            UiManager.Instance.animalFocusUi.buttonLevelUp.interactable = true;
        }

        //buttonText.text = animalData.Level == animalData.Level_Max ? mergeText : levelUpText;

        textAnimalName.text = animalData.GetName();
        textAnimalLevel.text = string.Format(levelFormat, animalData.Level, animalData.Level_Max);
        textCurrentFloor.text = string.Format(currentFloorFormat, animalWork.Animal.animalStat.CurrentFloor);
        textAnimalWorkload.text = string.Format(workloadFormat, animalData.Workload.ToBigNumber().ToString());

        if(animalWork.Animal.animalStat.Level_Up_Coin_Value.ToBigNumber().IsZero)
        {
            levelUpCostGo.SetActive(false);
            textLevelUpCost.gameObject.SetActive(false);
        }
        else
        {
            levelUpCostGo.SetActive(true);
            textLevelUpCost.gameObject.SetActive(true);
            BigNumber lvCoin = new BigNumber(animalWork.Animal.animalStat.AnimalData.Level_Up_Coin_Value);
            if (CurrencyManager.currency[CurrencyType.Coin] < lvCoin)
            {
                textLevelUpCost.color = Color.red;
            }
            else
            {
                textLevelUpCost.color = Color.white;
            }
            textLevelUpCost.text = animalWork.Animal.animalStat.Level_Up_Coin_Value.ToBigNumber().ToString();
        }
        textSellCost.text = animalWork.Animal.animalStat.Sale_Coin.ToBigNumber().ToString();
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
