using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Test : MonoBehaviour
{
    public LayerMask floorLayer;
    public AssetReference hamsterPrefabReference; // Addressable Asset Reference

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
                    CreateHamster(pos, spawnFloor);
            }
        }
    }

    private void CreateHamster(Vector3 position, Floor floor)
    {
        hamsterPrefabReference.InstantiateAsync(position, Quaternion.identity, floor.transform).Completed += (AsyncOperationHandle<GameObject> handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var animalWork = handle.Result.GetComponent<AnimalWork>();
                animalWork.currentFloor = floor.floorName;
                animalWork.Animal = new Animal(10105001);
                animalWork.Animal.SetAnimal();
                floor.animals.Add(animalWork.Animal);
                Debug.Log(DataTableMgr.GetAnimalTable().Get(10105001));
            }
        };
    }
}
