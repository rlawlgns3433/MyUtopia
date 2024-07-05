using UnityEngine;

public class Test : MonoBehaviour
{
    public LayerMask floorLayer;
    public GameObject hamsterPrefab;
    void Start()
    {
        DataTableMgr.GetStringTable();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out var hit, 200f, floorLayer.value))
            {
                var pos = hit.point;
                var spawnFloor = hit.collider.gameObject.GetComponent<Floor>();
                pos.y = 0f;

                if(spawnFloor != null)
                    CreateHamster(pos, spawnFloor);
            }
        }
    }

    private void CreateHamster(Vector3 posistion, Floor floor)
    {
        var go = Instantiate(hamsterPrefab, posistion, Quaternion.identity, floor.transform);
        var animalWork = go.GetComponent<AnimalWork>();
        floor.animals.AddLast(animalWork.animal);
    }
}
