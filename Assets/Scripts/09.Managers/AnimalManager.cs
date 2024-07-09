using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AnimalManager : MonoBehaviour
{
    public LayerMask floorLayer;
    public AssetReference hamsterPrefabReference; // Addressable Asset Reference
    public AssetReference ferretPrefabReference; // Addressable Asset Reference

    public void MoveAnimal(string toFloor)
    {
        var animalClick = ClickableManager.CurrentClicked as AnimalClick;

        if (animalClick == null)
            return;

        FloorManager.MoveAnimal(animalClick.AnimalWork.currentFloor, toFloor, animalClick.AnimalWork.Animal);
        animalClick.gameObject.SetActive(false);
        animalClick.gameObject.transform.SetParent(FloorManager.GetFloor(toFloor).transform);
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

    void Start()
    {
        DataTableMgr.GetStringTable();
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

                if (spawnFloor != null)
                    Create(pos, spawnFloor, hamsterPrefabReference);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, 200f, floorLayer.value))
            {
                var pos = hit.point;
                var spawnFloor = hit.collider.gameObject.GetComponent<Floor>();
                pos.y = 0f;

                if (spawnFloor != null)
                    Create(pos, spawnFloor, ferretPrefabReference);
            }
        }
    }

    public void Create(Vector3 position, Floor floor, AssetReference asset, bool isMerged = false)
    {
        asset.InstantiateAsync(position, Quaternion.identity, floor.transform).Completed += (AsyncOperationHandle<GameObject> handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var animalWork = handle.Result.GetComponent<AnimalWork>();
                Debug.Log(animalWork.gameObject.GetInstanceID());
                animalWork.currentFloor = floor.floorName;
                animalWork.Animal = new Animal(animalWork.animalId);
                animalWork.Animal.animalWork = animalWork;
                animalWork.Animal.SetAnimal();
                floor.animals.Add(animalWork.Animal);

                if(isMerged)
                {
                    var animalClick = handle.Result.GetComponent<AnimalClick>();
                    animalClick.IsClicked = true;
                }
            }
        };
    }
}
