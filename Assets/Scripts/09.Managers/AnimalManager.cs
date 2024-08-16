using Spine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AnimalManager : Subject
{
    public LayerMask floorLayer;
    public Dictionary<int, AssetReference> animalDictionary = new Dictionary<int, AssetReference>();
    public UiAnimalInventory uiAnimalInventory;
    public UiAnimalList uiAnimalList;
    public UiCurrencies uiCurrencies;
    public Observer uiWorldAnimalCount;

    private AnimalTable animalTable;
    public AnimalTable AnimalTable
    {
        get
        {
            if(animalTable == null)
            {
                animalTable = DataTableMgr.GetAnimalTable();
            }
            return animalTable;
        }
    }
    public void MoveAnimal(string fromFloor, string toFloor, Animal animal)
    {
        var animalClick = animal.animalWork.gameObject.GetComponent<AnimalClick>();

        if (animalClick == null)
            return;
        Debug.Log($"moveTest{animalClick.AnimalWork.Animal.animalStat.Animal_ID}");
        if(FloorManager.Instance.MoveAnimal(fromFloor, toFloor, animalClick.AnimalWork.Animal))
        {
            animalClick.gameObject.SetActive(false);
            animalClick.gameObject.GetComponent<AnimalController>().behaviorTreeRoot.InitializeBehaviorTree();
            animalClick.gameObject.transform.SetParent(FloorManager.Instance.GetFloor(toFloor).transform);
            animalClick.gameObject.transform.localPosition = -Vector3.forward * 3f;
            animalClick.gameObject.SetActive(true);


            //FloorManager.Instance.CheckFloorSynergy(FloorManager.Instance.GetFloor(fromFloor)); 시너지
            //FloorManager.Instance.CheckFloorSynergy(FloorManager.Instance.GetFloor(toFloor));
        }
    }

    public void MoveAnimal(string toFloor)
    {
        var animalClick = ClickableManager.CurrentClicked as AnimalClick;

        if (animalClick == null)
            return;
        Debug.Log($"moveTest{animalClick.AnimalWork.Animal.animalStat.Animal_ID}");
        if(FloorManager.Instance.MoveAnimal(animalClick.AnimalWork.Animal.animalStat.CurrentFloor, toFloor, animalClick.AnimalWork.Animal))
        {
            animalClick.gameObject.SetActive(false);
            animalClick.gameObject.transform.SetParent(FloorManager.Instance.GetFloor(toFloor).transform);
            animalClick.gameObject.transform.localPosition = Vector3.zero;
            animalClick.gameObject.SetActive(true);
        }
    }

    public void LevelUpAnimal()
    {
        var animalClick = ClickableManager.CurrentClicked as AnimalClick;

        if (animalClick == null)
            return;

        animalClick.AnimalWork.Animal.LevelUp();
    }

    private void Start()
    {
        DataTableMgr.GetStringTable();
        Attach(uiWorldAnimalCount);
    }

    public void CreateAnimal()
    {
        UiManager.Instance.ShowInvitationUi();
        UiManager.Instance.invitationUi.Set();
        NotifyObservers();
        if(FloorManager.Instance.touchManager.tutorial != null)
        {
            if (FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.PurchaseAnimal || FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.MurgeAnimalPurchase)
            {
                FloorManager.Instance.touchManager.tutorial.SetTutorialProgress();
            }
        }
    }

    public void Create(Vector3 position, Floor floor, int animalId, int slotId, bool isMerged = false)
    {
        if (!isMerged)
        {
            if (floor.animals.Count >= floor.FloorStat.Max_Population)
                return;
        }

        while (animalDictionary.Count == 0)
        {
            foreach (var animal in AnimalTable.GetKeyValuePairs)
            {
                animalDictionary.Add(animal.Key, new AssetReference(animal.Value.Prefab));
            }
        }

        var pos = position;
        //pos.z -= 3;

        animalDictionary[animalId].InstantiateAsync(pos, Quaternion.identity, floor.transform).Completed += (AsyncOperationHandle<GameObject> handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var animalWork = handle.Result.GetComponent<AnimalWork>();
                animalWork.Animal = new Animal(animalId);
                animalWork.Animal.animalStat.CurrentFloor = floor.floorName;
                Debug.Log($"{animalWork.Animal.animalStat.CurrentFloor}/{floor.floorName}");
                animalWork.Animal.animalWork = animalWork;

                animalWork.Animal.SetAnimal();

                floor.animals.Add(animalWork.Animal);

                if (isMerged)
                {
                    var animalClick = handle.Result.GetComponent<AnimalClick>();
                    animalClick.IsClicked = true;
                }
                uiAnimalInventory.UpdateInventory(isMerged);

                //FloorManager.Instance.CheckFloorSynergy(floor); 시너지
                NotifyObservers();
            }
            UiManager.Instance.animalFocusUi.Set();
            if(uiAnimalList.gameObject.activeSelf)
            {
                uiAnimalList.Refresh();
            }
        };
        CatalogueManager.Instance.UnlockAnimal(animalId);
    }

    public void Create(Vector3 position, Floor floor, int animalId, int slotId, AnimalStat animalStat, bool isMerged = false)
    {
        if (!isMerged)
        {
            if (floor.animals.Count >= floor.FloorStat.Max_Population)
                return;
        }

        while (animalDictionary.Count == 0)
        {
            foreach (var animal in AnimalTable.GetKeyValuePairs)
            {
                animalDictionary.Add(animal.Key, new AssetReference(animal.Value.Prefab));
            }
        }
        var pos = position;
        pos.z -= 3;

        animalDictionary[animalId].InstantiateAsync(pos, Quaternion.identity, floor.transform).Completed += (AsyncOperationHandle<GameObject> handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var animalWork = handle.Result.GetComponent<AnimalWork>();
                animalWork.Animal = new Animal(animalWork.animalId);
                animalWork.Animal.animalStat.CurrentFloor = floor.floorName;
                Debug.Log($"{animalWork.Animal.animalStat.CurrentFloor}/{floor.floorName}");
                animalWork.Animal.animalWork = animalWork;
                animalWork.Animal.animalStat = animalStat;

                animalWork.Animal.SetAnimal();

                floor.animals.Add(animalWork.Animal);

                if (isMerged)
                {
                    var animalClick = handle.Result.GetComponent<AnimalClick>();
                    animalClick.IsClicked = true;
                }
                uiAnimalInventory.UpdateInventory(isMerged);
                //uiCurrencies.SetAllAnimals();
                NotifyObservers();
            }
            UiManager.Instance.animalFocusUi.Set();
        };
    }
}
