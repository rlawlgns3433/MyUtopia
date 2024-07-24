using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using static UnityEngine.UI.CanvasScaler;

public class GameManager : Singleton<GameManager>
{
    private Dictionary<SceneIds, SceneController> sceneManagers = new Dictionary<SceneIds, SceneController>();
    //private Dictionary<SceneIds, UIManager> uiManagers = new Dictionary<SceneIds, UIManager>();
    private AnimalManager animalManager;
    private SceneIds currentSceneId;
    public SceneIds CurrentSceneId
    {
        get
        {
            return currentSceneId;
        }
        set
        {
            currentSceneId = value;
            //GetUIManager(currentSceneId).InitializeUI();
            GetSceneController(currentSceneId);
        }
    }

    private void Awake()
    {
        CurrencyManager.Init();
        CurrentSceneId = SceneIds.WorldLandOfHope;
    }

    private void OnApplicationQuit()
    {

        var saveData = new SaveDataV1();
        var saveCurrencyData = new SaveCurrencyDataV1();

        for(int i = 0; i < FloorManager.Instance.floors.Count; ++i)
        {
            string format = $"B{i + 1}";
            var floor = FloorManager.Instance.floors[format];
            saveData.floors.Add(new FloorSaveData(floor.FloorStat));
            foreach(var animal in floor.animals)
            {
                saveData.floors[saveData.floors.Count - 1].animalSaveDatas.Add(new AnimalSaveData(animal.animalStat));
            }

            foreach(var building in floor.buildings)
            {
                saveData.floors[saveData.floors.Count - 1].buildingSaveDatas.Add(new BuildingSaveData(building.BuildingStat));
            }

            if(floor.storage != null)
                saveData.floors[saveData.floors.Count - 1].furnitureSaveDatas.Add(new FurnitureSaveData(floor.storage.FurnitureStat)); // 창고
        }


        SaveLoadSystem.Save(saveData);

        for (int i = 0; i < CurrencyManager.currencyTypes.Length; ++i)
        {
            saveCurrencyData.currencySaveData.Add(new CurrencySaveData(CurrencyManager.currencyTypes[i], CurrencyManager.currency[CurrencyManager.currencyTypes[i]]));
        }

        SaveLoadSystem.Save(saveCurrencyData, 1);
    }

    private async void Start()
    {
        //RegisterSceneManager(SceneIds.WorldSelect, new WorldSelectManager());
        //RegisterSceneManager(SceneIds.WorldLandOfHope, new WorldLandOfHopeManager());
        await UniWaitTables();

        SaveDataV1 saveWorldData = SaveLoadSystem.Load() as SaveDataV1;

        // 데이터대로 동물, 건물, 가구 생성
        if (saveWorldData != null)
        {
            for (int i = 0; i < saveWorldData.floors.Count; ++i)
            {
                var floorSaveData = saveWorldData.floors[i];
                var animals = floorSaveData.animalSaveDatas;
                var buildings = floorSaveData.buildingSaveDatas;

                var floor = FloorManager.Instance.GetFloor($"B{floorSaveData.floorStat.Floor_Num}");
                floor.FloorStat = floorSaveData.floorStat;



                foreach (var animal in animals)
                {
                    var pos = floor.transform.position;
                    pos.z -= 5;
                    GetAnimalManager().Create(pos, floor, animal.animalStat.Animal_ID, 0, animal.animalStat);
                }

                int index = floorSaveData.floorStat.Floor_ID % 10 - 1;
                for (int j = 0; j <= index; ++j)
                {
                    if (buildings.Count == 0)
                        break;
                    floor.buildings[j].BuildingStat.IsLock = false;
                    floor.buildings[j].gameObject.SetActive(true);
                }

                for (int j = 0; j < floor.buildings.Count; ++j)
                {
                    floor.buildings[j].BuildingStat = buildings[j].buildingStat;
                }
            }
        }
        else
        {
            // 첫 접속일 때 Unlock된 건물 Unlock
            var floors = FloorManager.Instance.floors;

            foreach(var floor in floors.Values)
            {
                foreach(var building in floor.buildings)
                {
                    if(building.BuildingStat.Building_ID == floor.FloorStat.Unlock_Facility)
                    {
                        building.BuildingStat.IsLock = false;
                        break;
                    }
                }
            }
        }

        SaveCurrencyDataV1 saveCurrencyData = SaveLoadSystem.Load(1) as SaveCurrencyDataV1;
        if (saveCurrencyData != null)
        {
            for (int i = 0; i < CurrencyManager.currencyTypes.Length; ++i)
            {
                CurrencyManager.currency[CurrencyManager.currencyTypes[i]] = saveCurrencyData.currencySaveData[i].value;
            }
        }
    }

    public void RegisterSceneManager(SceneIds sceneName, SceneController sceneManager)
    {
        if (!sceneManagers.ContainsKey(sceneName))
        {
            sceneManagers[sceneName] = sceneManager;
            //uiManagers[sceneName] = new UIManager();
        }
    }

    public SceneController GetSceneController(SceneIds sceneName)
    {
        if (sceneManagers.ContainsKey(sceneName))
        {
            return sceneManagers[sceneName];
        }
        return null;
    }

    //public UIManager GetUIManager(SceneIds sceneName)
    //{
    //    if (uiManagers.ContainsKey(sceneName))
    //    {
    //        return uiManagers[sceneName];
    //    }
    //    return null;
    //}

    private void LoadSceneAsync(int sceneIndex)
    {
        CurrentSceneId = (SceneIds)sceneIndex;
        SceneManager.LoadSceneAsync(sceneIndex);
    }

    public AnimalManager GetAnimalManager()
    {
        if(CurrentSceneId == SceneIds.WorldLandOfHope)
        {
            if (animalManager == null)
            {
                animalManager = GameObject.FindWithTag(Tags.AnimalManager).GetComponentInChildren<AnimalManager>();
            }
            return animalManager;
        }
        throw new Exception("AnimalManager is not in the current scene");
    }

    public async UniTask UniWaitTables()
    {
        while (!DataTableMgr.GetAnimalTable().IsLoaded)
        {
            await UniTask.Yield();
        }
        
        while (!DataTableMgr.GetFloorTable().IsLoaded)
        {
            await UniTask.Yield();
        }
        
        while (!DataTableMgr.GetBuildingTable().IsLoaded)
        {
            await UniTask.Yield();
        }
        
        while (!DataTableMgr.GetFurnitureTable().IsLoaded)
        {
            await UniTask.Yield();
        }
        
        while (!DataTableMgr.GetItemTable().IsLoaded)
        {
            await UniTask.Yield();
        }   

        while (!DataTableMgr.GetInvitationTable().IsLoaded)
        {
            await UniTask.Yield();
        }

        while (!DataTableMgr.GetWorldTable().IsLoaded)
        {
            await UniTask.Yield();
        }
        return;
    }
}


public class WorldSelectManager : SceneController
{
    public override void Start()
    {
        base.Start();
    }
}

public class WorldLandOfHopeManager : SceneController
{
    public override void Start()
    {
        base.Start();
    }
}

public class SceneController : MonoBehaviour
{
    public virtual void Start()
    {
        Debug.Log("Base SceneController Start");
    }
}