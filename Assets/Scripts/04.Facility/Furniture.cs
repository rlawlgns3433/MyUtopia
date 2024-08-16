using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour
{
    private Floor currentFloor;
    public Floor CurrentFloor
    {
        get
        {
            if(currentFloor == null)
            {
                currentFloor = GetComponentInParent<Floor>();
            }
            return currentFloor;
        }
        set
        {
            currentFloor = value;
        }
    }
    public List<GameObject> furnitures = new List<GameObject>();

    private void OnEnable()
    {
        // ���� ������ ���� ����
        // Ʈ�� �籸��
        for (int i = 0; i < CurrentFloor.FloorStat.Grade; ++i)
        {
            furnitures[i].SetActive(true);
        }

        foreach (var animal in CurrentFloor.animals)
        {
            var controller = animal.animalWork.GetComponent<AnimalController>();
            controller.behaviorTreeRoot.InitializeBehaviorTree();
        }
    }

    public void Refresh()
    {
        for (int i = 0; i < CurrentFloor.FloorStat.Grade; ++i)
        {
            furnitures[i].SetActive(true);
        }
    }
}