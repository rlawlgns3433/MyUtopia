using System.Collections.Generic;
using UnityEngine;

// GameManger로 이동 필요
public class FloorManager : Singleton<FloorManager>
{
    public Dictionary<string, Floor> floors = new Dictionary<string, Floor>();
    private int currentFloorIndex;
    public int CurrentFloorIndex
    {
        get
        {
            return currentFloorIndex;
        }
        set
        {
            currentFloorIndex = value;
        }
    }
    private FloorMove floorMove;
    public FloorMove FloorMove
    {
        get
        {
            if(floorMove == null)
            {
                floorMove = GameObject.FindWithTag(Tags.Floors).GetComponent<FloorMove>();
            }

            return floorMove;
        }
    }

    public void AddFloor(string floorId, Floor floor)
    {
        if (floors.ContainsKey(floorId))
            return;

        floors[floorId] = floor;
    }

    public bool MoveAnimal(string fromFloor, string toFloor, Animal animal)
    {
        if (!floors.ContainsKey(fromFloor))
            return false;

        if (!floors.ContainsKey(toFloor))
            return false;

        var currentFloor = GetCurrentFloor();
        if (currentFloor.animals.Count >= currentFloor.FloorStat.Max_Population)
            return false;

        animal.animalWork.Animal.animalStat.CurrentFloor = toFloor;
        floors[toFloor].animals.Add(animal);
        animal.animalWork.MoveFloor();
        floors[fromFloor].animals.Remove(animal);
        UiManager.Instance.animalFocusUi.Set();
        return true;
    }

    public Floor GetFloor(string floorId)
    {
        if (!floors.ContainsKey(floorId))
            return null;
        return floors[floorId];
    }

    public void SetFloor(string floorId)
    {
        CurrentFloorIndex = int.Parse(floorId[1].ToString());
        FloorMove.UpCount = CurrentFloorIndex;
    }

    public Floor GetCurrentFloor()
    {
        var floorId = $"B{CurrentFloorIndex}";
        if (!floors.ContainsKey(floorId))
            return null;
        return floors[floorId];
    }

    public void LevelUp(string floorId)
    {
        if (!floors.ContainsKey(floorId))
            return;
        floors[floorId].LevelUp();
    }

    public void LevelUp()
    {
        if(GetCurrentFloor() != null)
        {
            floors[$"B{CurrentFloorIndex}"].LevelUp();
        }
    }
}
