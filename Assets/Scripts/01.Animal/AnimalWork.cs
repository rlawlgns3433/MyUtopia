using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class AnimalWork : Subject, IMergable
{
    public Observer uiSlot;
    public Observer uiAnimalFloorSlot;
    private AnimalManager animalManager;
    private CancellationTokenSource cts = new CancellationTokenSource();
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

    private void OnDestroy()
    {
        cts.Cancel();
        cts.Dispose();

        Detach(uiSlot);
        Detach(uiAnimalFloorSlot);

        if(uiSlot != null)
            Destroy(uiSlot.gameObject);
        if(uiAnimalFloorSlot != null)
            Destroy(uiAnimalFloorSlot.gameObject);
    }

    private void Start()
    {
        animalManager = GameManager.Instance.GetAnimalManager();
        Attach(uiSlot);
        Attach(uiAnimalFloorSlot);
        UniConsumeStamina(cts.Token).Forget();
    }

    public bool Merge(AnimalWork animalWork)
    {
        if (ClickableManager.CurrentClicked as AnimalClick == null)
            return false;

        if (gameObject.Equals(animalWork.gameObject))
            return false;

        if (animalWork.animal.animalStat.Animal_ID== animal.animalStat.Animal_ID)
        {
            var floor = FloorManager.Instance.GetFloor(animalWork.currentFloor);
            int resultAnimalId = DataTableMgr.GetMergeTable().Get(animal.animalStat.Merge_ID).Result_Animal;
            animalManager.Create(floor.gameObject.transform.position, floor, resultAnimalId, 0, true);
            FloorManager.Instance.GetFloor(currentFloor).RemoveAnimal(animal);
            FloorManager.Instance.GetFloor(currentFloor).RemoveAnimal(animalWork.animal);
            Destroy(gameObject);
            Destroy(animalWork.gameObject);
            return true;
        }
        return false;
    }

    private async UniTaskVoid UniConsumeStamina(CancellationToken cts)
    {
        while (Animal.animalStat.Stamina > 0)
        {
            Animal.animalStat.Stamina -= 1;
            NotifyObservers();
            await UniTask.Delay(30, false, PlayerLoopTiming.Update, cts);
        }
    }
}