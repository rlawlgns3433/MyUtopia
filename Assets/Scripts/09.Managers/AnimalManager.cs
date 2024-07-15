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

    public void MoveAnimal(string toFloor)
    {
        var animalClick = ClickableManager.CurrentClicked as AnimalClick;

        if (animalClick == null)
            return;
        Debug.Log($"moveTest{animalClick.AnimalWork.Animal.animalStat.Animal_ID}");
        FloorManager.Instance.MoveAnimal(animalClick.AnimalWork.Animal.animalStat.CurrentFloor, toFloor, animalClick.AnimalWork.Animal);
        animalClick.gameObject.SetActive(false);
        animalClick.gameObject.transform.SetParent(FloorManager.Instance.GetFloor(toFloor).transform);
        animalClick.gameObject.transform.localPosition = Vector3.zero;
        animalClick.gameObject.SetActive(true);
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, 200f, floorLayer.value))
            {
                var pos = hit.point;
                var spawnFloor = hit.collider.gameObject.GetComponent<Floor>();
                pos.y = 0f;

                if (spawnFloor.animals.Count >= spawnFloor.FloorData.Max_Population)
                    return;

                if (spawnFloor != null)
                uiAnimalInventory.SetFloor(spawnFloor);
                NotifyObservers();
            }
        }
    }

    public void Create(Vector3 position, Floor floor, int animalId, int slotId, bool isMerged = false)
    {
        if (!isMerged)
        {
            if (floor.animals.Count >= floor.FloorData.Max_Population)
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
}
