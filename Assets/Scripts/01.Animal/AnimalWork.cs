using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class AnimalWork : Subject, IMergable
{
    public Observer uiAnimalFloorSlot;
    private AnimalManager animalManager;
    public CancellationTokenSource cts = new CancellationTokenSource();
    public int animalId;
    public bool isHealing = false;
    private int consumeStamina = -100;
    //public float staminaReductionRate = 0f; 시너지
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

    private void OnDestroy()
    {
        cts.Cancel();
        cts.Dispose();

        Detach(uiAnimalFloorSlot);

        if(uiAnimalFloorSlot != null)
            Destroy(uiAnimalFloorSlot.gameObject);
    }

    private void Start()
    {
        animalManager = GameManager.Instance.GetAnimalManager();
        StartConsumeStamina().Forget();
    }

    public bool Merge(AnimalWork animalWork)
    {
        if (ClickableManager.CurrentClicked as AnimalClick == null)
            return false;

        if (gameObject.Equals(animalWork.gameObject))
            return false;

        if (animalWork.animal.animalStat.Animal_ID== animal.animalStat.Animal_ID)
        {
            var floor = FloorManager.Instance.GetFloor(animal.animalStat.CurrentFloor);
            int resultAnimalId = DataTableMgr.GetMergeTable().Get(animal.animalStat.Merge_ID).Result_Animal;
            animalManager.Create(floor.gameObject.transform.position, floor, resultAnimalId, 0, true);
            FloorManager.Instance.GetFloor(animal.animalStat.CurrentFloor).RemoveAnimal(animal);
            FloorManager.Instance.GetFloor(animalWork.animal.animalStat.CurrentFloor).RemoveAnimal(animalWork.animal);
            //FloorManager.Instance.CheckFloorSynergy(FloorManager.Instance.GetFloor(animal.animalStat.CurrentFloor)); 시너지
            //FloorManager.Instance.CheckFloorSynergy(FloorManager.Instance.GetFloor(animalWork.animal.animalStat.CurrentFloor));
            Destroy(gameObject);
            Destroy(animalWork.gameObject);
            if(FloorManager.Instance.touchManager.tutorial != null)
            {
                if (FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.Murge)
                {
                    FloorManager.Instance.touchManager.tutorial.SetTutorialProgress();
                }
            }
            MissionManager.Instance.AddMissionCountMurge();
            return true;
        }
        return false;
    }

    public void MoveFloor()
    {
        StartConsumeStamina().Forget();
    }

    private async UniTaskVoid StartConsumeStamina()
    {
        cts.Cancel();
        cts = new CancellationTokenSource();
        await UniConsumeStamina(cts.Token);
    }
    private async UniTask UniConsumeStamina(CancellationToken cts)
    {
        Debug.Log("B2?else?"+Animal.animalStat.CurrentFloor);
        switch (animal.animalStat.CurrentFloor)
        {
            case "B1":
                NotifyObservers();
                break;
            case "B2":
                Debug.Log("B2Start");
                while (Animal.animalStat.Stamina < Animal.animalStat.AnimalData.Stamina)
                {
                    if (cts.IsCancellationRequested)
                        break;
                    Floor floor = FloorManager.Instance.GetFloor(Animal.animalStat.CurrentFloor);
                    Animal.animalStat.Stamina += floor.FloorStat.Stamina_Recovery; 
                    NotifyObservers();
                    await UniTask.Delay(1000, false, PlayerLoopTiming.Update, cts);
                }
                break;
            default:
                Debug.Log("else");
                while (Animal.animalStat.Stamina > 0)
                {
                    if (cts.IsCancellationRequested)
                        break;
                    //Animal.animalStat.Stamina = Animal.animalStat.Stamina - 1 + staminaReductionRate; 시너지

                    Floor floor = FloorManager.Instance.GetFloor(Animal.animalStat.CurrentFloor);
                    consumeStamina = floor.FloorStat.Stamina_Recovery;
                    Animal.animalStat.Stamina += consumeStamina;

                    if(Animal.animalStat.Stamina < 1f)
                    {
                        animalManager.MoveAnimal(Animal.animalStat.CurrentFloor, "B2", Animal);
                        UiManager.Instance.mainUi.animalInventoryUi.UpdateInventory();
                    }

                    NotifyObservers();
                    await UniTask.Delay(1000, false, PlayerLoopTiming.Update, cts);
                }
                break;
        }
        
    }

    public void SetUiAnimalFloorSlot(Observer observer)
    {
        Attach(observer);
    }

    public void SetUiAnimalSlot(Observer observer)
    {
        Attach(observer);
    }

    public void AttachObserver(Observer observer)
    {
        Attach(observer);
    }

    public void DetachObserver(Observer observer)
    {
        Detach(observer);
    }
}