using System.Collections.Generic;
using System.Diagnostics;

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
    public void AddFloor(string floorId, Floor floor)
    {
        if (floors.ContainsKey(floorId))
            return;

        floors[floorId] = floor;
    }

    public void MoveAnimal(string fromFloor, string toFloor, Animal animal)
    {
        if (!floors.ContainsKey(fromFloor))
            return;

        if (!floors.ContainsKey(toFloor))
            return;
        animal.animalWork.Animal.animalStat.CurrentFloor = toFloor;
        floors[toFloor].animals.Add(animal);
        animal.animalWork.MoveFloor();
        floors[fromFloor].animals.Remove(animal);
        UiManager.Instance.animalFocusUi.Set();
    }

    public Floor GetFloor(string floorId)
    {
        if (!floors.ContainsKey(floorId))
            return null;
        return floors[floorId];
    }

    public void SetFloor(string floorId)
    {
        CurrentFloorIndex = floorId[1];
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
