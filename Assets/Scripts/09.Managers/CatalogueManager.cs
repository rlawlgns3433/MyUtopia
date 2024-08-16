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
    public int id;
    public bool isLock;
}
[Serializable]
public class SaveCatalogueData
{
    public List<CatalogueData> catalogueDatas;
    public bool isGetFirstAnimal;
}

public class CatalogueManager : Singleton<CatalogueManager>
{
    private Dictionary<int, AnimalData> animalDictionary = new Dictionary<int, AnimalData>();
    private Dictionary<int, bool> animalCatalogue = new Dictionary<int, bool>();
    private List<CatalogueData> catalogueDatas = new List<CatalogueData>();
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
    public bool firstGetAnimal = false;
    public GameObject isFirstGetAnimal;
    private bool isAddQuitEvent = false;
    private void Awake()
    {
        if (!isAddQuitEvent)
        {
            Application.quitting -= SaveCatalougeData;
            Application.quitting += SaveCatalougeData;
            isAddQuitEvent = true;
        }
    }
    private async void Start()
    {
        await GameManager.Instance.UniWaitTables();
        animalTable = DataTableMgr.GetAnimalTable();
        animalDataList = new List<AnimalData>(animalTable.Count);
        LoadCatalougeData();
        if (catalogueDatas.Count <= 0)
        {
            LoadAnimal();
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            SaveCatalougeData();
        }
    }

    private void LoadAnimal()
    {
        foreach (var animalData in animalTable.GetKeyValuePairs.Values)
        {
            if (animalData.Animal_ID % 10 == 1 && (animalData.Animal_ID / 10) % 10 == 0)
            {
                animalDictionary.Add(animalData.Animal_ID, animalData);
                animalCatalogue.Add(animalData.Animal_ID,false);
                animalDataList.Add(animalData); //¾ê°¡ 0ÀÌ³ª¿È
            }
        }
        firstGetAnimal = false;
        count = GetMaxAnimalType();
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
            UiManager.Instance.SetCatalougeImage(firstGetAnimal);
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

    public void UpdateAnimals()
    {
        for(int i = 1; i <= FloorManager.Instance.floors.Count; ++i)
        {
            var floor = FloorManager.Instance.GetFloor($"B{i}");
            foreach(var animal in floor.animals)
            {
                var data = animalTable.Get(animal.animalStat.Animal_ID);
                var result = animalDictionary.Values.FirstOrDefault(animal => animal.Animal_Name_ID == data.Animal_Name_ID);
                if (animalCatalogue.ContainsKey(result.Animal_ID))
                {
                    if (!animalCatalogue[result.Animal_ID])
                    {
                        animalCatalogue[result.Animal_ID] = true;
                        firstGetAnimal = true;
                        UiManager.Instance.SetCatalougeImage(firstGetAnimal);
                    }
                }
            }
        }
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
                id = data.Key,
                isLock = data.Value
            }).ToList()
        };
        saveData.isGetFirstAnimal = firstGetAnimal;
        SaveLoadSystem.Save(saveData);
    }

    public void LoadCatalougeData()
    {
        SaveCatalogueData gameData = SaveLoadSystem.CatalougeLoad();
        if(gameData == null)
        {
            LoadAnimal();
            return;
        }
        
        catalogueDatas = gameData.catalogueDatas;
        firstGetAnimal = gameData.isGetFirstAnimal;
        for (int i = 0; i < catalogueDatas.Count; i++)
        {
            animalCatalogue[catalogueDatas[i].id] = catalogueDatas[i].isLock;
            var animalData = animalTable.Get(catalogueDatas[i].id);
            animalDictionary.Add(catalogueDatas[i].id, animalData);
            animalDataList.Add(animalTable.Get(catalogueDatas[i].id));
        }
        count = GetMaxAnimalType();
        UiManager.Instance.SetCatalougeImage(firstGetAnimal);
        //animalCatalogue.Clear();
        //foreach (var saveData in gameData.catalogueDatas)
        //{
        //    var missionData = DataTableMgr.GetAnimalTable().Get(saveData.id);
        //}
    }
}
