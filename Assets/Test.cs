using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Test : MonoBehaviour
{
    public LayerMask floorLayer;
    public AssetReferenceGameObject hamsterPrefabReference; // Addressable Asset Reference
    private GameObject hamsterPrefab;

    void Start()
    {
        DataTableMgr.GetStringTable();

        // Load the prefab and store the reference
        hamsterPrefabReference.LoadAssetAsync<GameObject>().Completed += (AsyncOperationHandle<GameObject> handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                hamsterPrefab = handle.Result;
            }
            else
            {
                Debug.LogError("Failed to load Hamster prefab.");
            }
        };
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
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
                floor.animals.Add(animalWork.myAnimalData);
            }
            else
            {
                Debug.LogError("Failed to instantiate Hamster prefab.");
            }
        };
    }
}
