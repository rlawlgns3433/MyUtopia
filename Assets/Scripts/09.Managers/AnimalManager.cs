using Cysharp.Threading.Tasks;
using Spine;
using System;
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
    public List<Observer> runtimeObservers = new List<Observer>();

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
    public void MoveAnimal(string fromFloor, string toFloor, Animal animal, bool isExchange = false)
    {
        var animalClick = animal.animalWork.gameObject.GetComponent<AnimalClick>();

        if (animalClick == null)
            return;
        Debug.Log($"moveTest{animalClick.AnimalWork.Animal.animalStat.Animal_ID}");
        if(FloorManager.Instance.MoveAnimal(fromFloor, toFloor, animalClick.AnimalWork.Animal, isExchange))
        {
            animalClick.gameObject.SetActive(false);
            animalClick.gameObject.GetComponent<AnimalController>().BehaviorTreeRoot.InitializeBehaviorTree();
            animalClick.gameObject.transform.SetParent(FloorManager.Instance.GetFloor(toFloor).transform);
            animalClick.gameObject.transform.localPosition = -Vector3.forward * 3f;
            animalClick.gameObject.SetActive(true);
            //FloorManager.Instance.CheckFloorSynergy(FloorManager.Instance.GetFloor(fromFloor)); 시너지
            //FloorManager.Instance.CheckFloorSynergy(FloorManager.Instance.GetFloor(toFloor));
        }
        NotifyObservers();
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

        foreach(var ob in runtimeObservers)
        {
            Attach(ob);
        }
    }

    public void CreateAnimal()
    {
        UiManager.Instance.ShowInvitationUi();
        UiManager.Instance.invitationUi.Set();
        NotifyObservers();
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
                var now = DateTime.Now;
                animalWork.Animal.animalStat.AcquireTime = now.Hour * 3600 + now.Minute * 60 + now.Second;

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

    public void Create(Vector3 position, Floor floor, int animalId, int slotId, AnimalStat animalStat, bool isMerged = false, bool isLoaded = true)
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

                if(!isLoaded)
                {
                    var now = DateTime.Now;
                    animalStat.AcquireTime = now.Day * 24 * 3600 + now.Hour * 3600 + now.Minute * 60 + now.Second;
                }
                else
                {
                    MoveAnimal(animalWork.Animal.animalStat.CurrentFloor, floor.floorName, animalWork.Animal);
                }

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
