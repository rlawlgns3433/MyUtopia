using Cysharp.Threading.Tasks;
using Spine;
using UnityEngine;
using UnityEngine.UI;

public class AnimalWork : Subject, IMergable
{
    public Observer uiSlot;
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

    public string currentFloor;

    private void Start()
    {
        animalManager = GameManager.Instance.GetAnimalManager();
        Attach(uiSlot);
        UniConsumeStamina().Forget();
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
            int resultAnimalId = DataTableMgr.GetMergeTable().Get(animal.animalData.Merge_ID).Result_Animal;
            animalManager.Create(floor.gameObject.transform.position, floor, resultAnimalId, 0, true);
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
        while (Animal.animalData.Stamina > 0)
        {
           Animal.animalData.Stamina -= 1;
            //NotifyObservers();
            await UniTask.Delay(30);
        }
    }
}