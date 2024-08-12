using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
[Serializable]
public struct CatalogueData
{
    public AnimalData animal;
    public bool isLock;
}
[Serializable]
public struct SaveCatalogueData
{
    public List<CatalogueData> catalogueDatas;
}

public class CatalogueManager : Singleton<CatalogueManager>
{
    private Dictionary<int, AnimalData> animalDictionary = new Dictionary<int, AnimalData>();
    private Dictionary<int, bool> animalCatalogue = new Dictionary<int, bool>();
    private List<AnimalData> animalDataList;
    public List<AnimalData> AnimalDataList
    {
        get
        {
            return animalDataList;
        }
        private set
        {
            animalDataList = value;
        }
    }
    public AnimalTable animalTable;
    public int count = 0;
    private bool firstGetAnimal = false;
    public GameObject isFirstGetAnimal;
    private async void Start()
    {
        await GameManager.Instance.UniWaitTables();
        animalTable = DataTableMgr.GetAnimalTable();
        animalDataList = new List<AnimalData>(animalTable.Count);
        LoadAnimal();
        count = GetMaxAnimalType();
    }

    private void LoadAnimal()
    {
        foreach (var animalData in animalTable.GetKeyValuePairs.Values)
        {
            if (animalData.Animal_ID % 10 == 1 && (animalData.Animal_ID / 10) % 10 == 0)
            {
                animalDictionary.Add(animalData.Animal_ID, animalData);
                animalCatalogue.Add(animalData.Animal_ID,false);
                animalDataList.Add(animalData);
            }
        }
    }

    public bool HasAnimal(int animalID)
    {
        return animalCatalogue.ContainsKey(animalID);
    }

    public bool UnlockAnimal(int animalID)
    {
        if (HasAnimal(animalID) && !animalCatalogue[animalID])
        {
            animalCatalogue[animalID] = true;
            firstGetAnimal = true;
            return true;
        }
        return false;
    }

    public bool CheckAnimal(int animalID)
    {
        if (animalCatalogue[animalID])
        {
            return animalCatalogue[animalID];
        }
        return false;
    }

    private int GetMaxAnimalType()
    {
        if (animalDictionary.Count == 0)
        {
            return 0;
        }
        return animalDictionary.Values.Max(animal => animal.Animal_Type);
    }

    public void SaveCatalougeData()
    {
        SaveCatalogueData saveData = new SaveCatalogueData
        {
            catalogueDatas = animalCatalogue.Select(data => new CatalogueData
            {
                animal = animalDictionary[data.Key],
                isLock = data.Value
            }).ToList()
        };
    }


}
