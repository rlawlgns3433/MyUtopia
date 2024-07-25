using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class AnimalWork : Subject, IMergable
{
    public Observer uiSlot;
    public Observer uiAnimalFloorSlot;

    public Canvas canvasSpeech;
    public TextMeshProUGUI textSpeech;
    private AnimalManager animalManager;
    public CancellationTokenSource cts = new CancellationTokenSource();
    public int animalId;
    public bool isHealing = false;
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
            FloorManager.Instance.GetFloor(animal.animalStat.CurrentFloor).RemoveAnimal(animalWork.animal);
            Destroy(gameObject);
            Destroy(animalWork.gameObject);
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
                textSpeech.text = "식당";
                NotifyObservers();
                break;
            case "B2":
                textSpeech.text = "휴식중";
                Debug.Log("B2Start");
                while (Animal.animalStat.Stamina < Animal.animalStat.AnimalData.Stamina)
                {
                    if (cts.IsCancellationRequested)
                        break;
                    Animal.animalStat.Stamina += 1;
                    NotifyObservers();
                    await UniTask.Delay(1000, false, PlayerLoopTiming.Update, cts);
                }
                break;
            default:
                textSpeech.text = "작업중";
                Debug.Log("else");
                while (Animal.animalStat.Stamina > 0)
                {
                    if (cts.IsCancellationRequested)
                        break;
                    Animal.animalStat.Stamina -= 1;
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