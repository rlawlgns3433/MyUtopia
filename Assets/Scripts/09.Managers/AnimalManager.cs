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
            animalClick.gameObject.transform.SetParent(FloorManager.Instance.GetFloor(toFloor).transform);
            animalClick.gameObject.transform.localPosition = -Vector3.forward * 3f;
            animalClick.gameObject.SetActive(true);
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
        Attach(uiAnimalInventory);
    }

    public void CreateAnimal()
    {
        NotifyObservers();

    }

    public void Create(Vector3 position, Floor floor, int animalId, int slotId, bool isMerged = false)
    {
        if (!isMerged)
        {
            if (floor.animals.Count >= floor.FloorStat.Max_Population)
                return;
        }

        if (animalDictionary.Count == 0)
        {
            foreach (var animal in AnimalTable.GetKeyValuePairs)
            {
                animalDictionary.Add(animal.Key, new AssetReference(animal.Value.Prefab));
            }
        }

        animalDictionary[animalId].InstantiateAsync(position, Quaternion.identity, floor.transform).Completed += (AsyncOperationHandle<GameObject> handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var animalWork = handle.Result.GetComponent<AnimalWork>();
                animalWork.Animal = new Animal(animalWork.animalId);
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
                uiCurrencies.SetAllAnimals();
            }
            UiManager.Instance.animalFocusUi.Set();
        };
    }

    public void Create(Vector3 position, Floor floor, int animalId, int slotId, AnimalStat animalStat, bool isMerged = false)
    {
        if (!isMerged)
        {
            if (floor.animals.Count >= floor.FloorStat.Max_Population)
                return;
        }

        if (animalDictionary.Count == 0)
        {
            foreach (var animal in AnimalTable.GetKeyValuePairs)
            {
                animalDictionary.Add(animal.Key, new AssetReference(animal.Value.Prefab));
            }
        }

        animalDictionary[animalId].InstantiateAsync(position, Quaternion.identity, floor.transform).Completed += (AsyncOperationHandle<GameObject> handle) =>
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
                uiCurrencies.SetAllAnimals();
            }
            UiManager.Instance.animalFocusUi.Set();
        };
    }
}
