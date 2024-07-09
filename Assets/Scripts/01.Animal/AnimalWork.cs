using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class AnimalWork : MonoBehaviour, IMergable
{
    private AnimalManager animalManager;
    public int animalId;
    private Animal animal;
    public Animal Animal
    {
        get
        {
            if(animal == null)
            {
                animal = new Animal(animalId);
            }
            return animal;
        }
        set
        {
            animal = value;
        }
    }

    [SerializeField]
    private int grade;
    public int Grade { get => grade; set => grade = value; }
    [SerializeField]
    private AnimalType type;
    public AnimalType Type { get => type; set => type = value; }
    [SerializeField]
    private int mergeId;
    public int MergeId { get => mergeId; set => mergeId = value; }

    public Slider staminaSlider;
    public string currentFloor;

    private void Awake()
    {
        Animal = new Animal(animalId);
    }

    private void Start()
    {
        staminaSlider.maxValue = Animal.Stamina;
        staminaSlider.minValue = 0f;
        staminaSlider.value = Animal.Stamina;

        staminaSlider.onValueChanged.AddListener(
        (float value) =>
        {
            Animal.Stamina = (int)value;
        });

        //animalManager = GameObject.FindWithTag("AddressableLoad").GetComponent<AnimalManager>();
        animalManager = GameManager.Instance.GetAnimalManager();

        UniConsumeStamina().Forget();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            foreach(var a in FloorManager.GetFloor(currentFloor).animals)
            {
                if (Merge(a.animalWork))
                    break;
            }
        }
    }

    public bool Merge(AnimalWork animalWork)
    {
        if (ClickableManager.CurrentClicked as AnimalClick == null)
            return false;

        if (gameObject.Equals(animalWork.gameObject))
            return false;

        if (animalWork.animal.animalData.ID== animal.animalData.ID)
        {
            var floor = FloorManager.GetFloor(animalWork.currentFloor);
            animalManager.Create(floor.gameObject.transform.position, floor, animalManager.ferretPrefabReference);
            FloorManager.GetFloor(currentFloor).RemoveAnimal(animal);
            FloorManager.GetFloor(currentFloor).RemoveAnimal(animalWork.animal);
            Destroy(gameObject);
            Destroy(animalWork.gameObject);
            return true;
        }
        return false;
    }

    private async UniTaskVoid UniConsumeStamina()
    {
        while (Animal.Stamina > 0)
        {
            Animal.Stamina -= 1;
            staminaSlider.value = Animal.Stamina;

            await UniTask.Delay(3000);
        }
    }
}