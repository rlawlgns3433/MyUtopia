using System.Collections.Generic;

public class FloorManager
{
    public static Dictionary<string, Floor> floors = new Dictionary<string, Floor>();

    public static void AddFloor(string floorId, Floor floor)
    {
        if (floors.ContainsKey(floorId))
            return;

        floors[floorId] = floor;
    }

    public static void MoveAnimal(string fromFloor, string toFloor, Animal animal)
    {
        if (!floors.ContainsKey(fromFloor))
            return;

        if (!floors.ContainsKey(toFloor))
            return;

        floors[toFloor].animals.Add(animal);
        floors[fromFloor].animals.Remove(animal);
    }

    public static Floor GetFloor(string floorId)
    {
        if (!floors.ContainsKey(floorId))
            return null;
        return floors[floorId];
    }
}
