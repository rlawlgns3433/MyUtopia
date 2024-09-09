using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>, ISingletonCreatable
{
    private Dictionary<SceneIds, SceneController> sceneManagers = new Dictionary<SceneIds, SceneController>();
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
            GetSceneController(currentSceneId);
        }
    }

    public bool isPlayingData;
    public bool isLoadedWorld;
    public bool hasPatronBoardDateTime;
    private void Awake()
    {
        if (!ShouldBeCreatedInScene(SceneManager.GetActiveScene().name))
        {
            Destroy(gameObject);
            return;
        }
        Application.targetFrameRate = 60;
        //Application.quitting += SetPlayerData; // ����� ���� X
        CurrencyManager.Init();
        CurrentSceneId = SceneIds.WorldLandOfHope;
    }

    private void OnApplicationPause(bool pause) // ����� ���� 
    {
        if (pause)
        {
            SetPlayerData();
        }
    }


    private async void Start()
    {
        await UniWaitTables();
        await UniLoadWorldData();
    }

    public async UniTask UniLoadWorldData()
    {
        int count = 0;
        await UniTask.WaitUntil(
            () =>
            {
                if (++count > 20)
                    return true;

                return UtilityTime.Seconds > 0;
            });

        SaveDataV1 saveWorldData = SaveLoadSystem.Load() as SaveDataV1;

        if (saveWorldData != null)
        {
            for (int i = 0; i < saveWorldData.floors.Count; ++i)
            {
                var floorSaveData = saveWorldData.floors[i];
                var animals = floorSaveData.animalSaveDatas;
                var buildings = floorSaveData.buildingSaveDatas;

                var floor = FloorManager.Instance.GetFloor($"B{floorSaveData.floorStat.Floor_Num}");
                floor.FloorStat = floorSaveData.floorStat;

                if (floor.FloorStat.IsUpgrading)
                {
                    floor.FloorStat.UpgradeTimeLeft -= UtilityTime.Seconds;
                    if (floor.FloorStat.UpgradeTimeLeft < 0)
                    {
                        floor.FloorStat.UpgradeTimeLeft = 0;
                    }
                }

                for (int j = 0; j < floor.FloorStat.Grade; ++j)
                {
                    if (j >= floor.furniture.furnitures.Count)
                        break;

                    floor.furniture.furnitures[j].SetActive(true);
                }

                StorageConduct storageConduct = null;
                if ((floor as BuildingFloor) != null)
                {
                    storageConduct = (floor as BuildingFloor).storageConduct;
                }

                foreach (var animal in animals)
                {
                    var pos = floor.transform.position;
                    pos.z -= 5;
                    if (floorSaveData.floorStat.Floor_Num == 2)
                    {
                        animal.animalStat.Stamina += UtilityTime.Seconds;
                    }
                    if (floorSaveData.floorStat.Floor_Num >= 3)
                    {
                        var currentSutamina = animal.animalStat.Stamina;
                        animal.animalStat.Stamina -= UtilityTime.Seconds;
                        if (animal.animalStat.Stamina <= 0)
                        {
                            if (storageConduct != null)
                            {
                                storageConduct.OffLineWorkLoad += Mathf.Abs(currentSutamina);
                            }
                            //animal.animalStat.Stamina = 0;
                        }
                    }
                    if (animal.animalStat.Stamina > 0)
                    {
                        animal.animalStat.Stamina = Mathf.Min(animal.animalStat.AnimalData.Stamina, animal.animalStat.Stamina);

                        GetAnimalManager().Create(pos, floor, animal.animalStat.Animal_ID, 0, animal.animalStat);
                    }
                    else if (animal.animalStat.Stamina <= 0)
                    {
                        var moveFloor = FloorManager.Instance.GetFloor("B2");
                        animal.animalStat.CurrentFloor = "B2";
                        animal.animalStat.Stamina = Mathf.Abs(animal.animalStat.Stamina);
                        var animalMaxSutamina = DataTableMgr.GetAnimalTable().Get(animal.animalStat.Animal_ID).Stamina;
                        if(animal.animalStat.Stamina >= animalMaxSutamina)
                        {
                            animal.animalStat.Stamina = animalMaxSutamina;
                        }
                        GetAnimalManager().Create(pos, moveFloor, animal.animalStat.Animal_ID, 0, animal.animalStat);
                    }
                }

                int index = floorSaveData.floorStat.Floor_ID % 10 - 1;
                for (int j = 0; j <= index; ++j)
                {
                    if (buildings.Count == 0)
                        break;

                    if (buildings[j].buildingStat.IsUpgrading)
                    {
                        buildings[j].buildingStat.UpgradeTimeLeft -= UtilityTime.Seconds;
                        if (buildings[j].buildingStat.UpgradeTimeLeft < 0)
                        {
                            buildings[j].buildingStat.UpgradeTimeLeft = 0;
                        }
                    }

                    buildings[j].buildingStat.IsLock = false;
                    floor.buildings[j].gameObject.SetActive(true);
                }

                for (int j = 0; j < floor.buildings.Count; ++j)
                {
                    floor.buildings[j].BuildingStat = buildings[j].buildingStat;
                }
                //if (storageConduct != null)
                //{
                //    await storageConduct.CheckStorage();
                //}
            }
        }
        else
        {
            var floors = FloorManager.Instance.floors;

            foreach (var floor in floors.Values)
            {
                foreach (var building in floor.buildings)
                {
                    if (building.BuildingStat.Building_ID == floor.FloorStat.Unlock_Facility)
                    {
                        building.BuildingStat = new BuildingStat(building.buildingId);
                        building.BuildingStat.IsLock = false;
                        break;
                    }
                }
            }

            //if (UiManager.Instance.testPanelUi == null)
            //{
            //    Debug.Log("testPanel Null");
            //}
            //UiManager.Instance.testPanelUi.SetEmptyData();
            UiManager.Instance.productsUi.capacity = 6;
        }

        SaveCurrencyDataV1 saveCurrencyData = SaveLoadSystem.Load(SaveLoadSystem.SaveType.Currency) as SaveCurrencyDataV1;
        if (saveCurrencyData != null)
        {
            for (int i = 0; i < CurrencyManager.currencyTypes.Length; ++i)
            {
                CurrencyManager.currency[CurrencyManager.currencyTypes[i]] = saveCurrencyData.currencySaveData[i].value;
            }
        }

        SaveCurrencyProductDataV1 saveCurrencyProductData = SaveLoadSystem.Load(SaveLoadSystem.SaveType.CurrencyProduct) as SaveCurrencyProductDataV1;
        if (saveCurrencyProductData != null)
        {
            for (int i = 0; i < CurrencyManager.productTypes.Length; ++i)
            {
                CurrencyManager.product[CurrencyManager.productTypes[i]] = saveCurrencyProductData.currencySaveData[i].value;
            }
        }

        SaveProductDataV1 saveProductData = SaveLoadSystem.Load(SaveLoadSystem.SaveType.Product) as SaveProductDataV1;
        if (saveProductData != null)
        {
            var storageProduct = FloorManager.Instance.GetFloor("B3").storage as StorageProduct;
            for (int i = 0; i < saveProductData.productSaveData.Count; ++i)
            {
                storageProduct.IncreaseProduct(saveProductData.productSaveData[i].productId, saveProductData.productSaveData[i].productValue);
            }
            UiManager.Instance.productsUi.capacity = storageProduct.BuildingStat.Effect_Value;
        }

        SavePatronDataV1 savePatronBoard = SaveLoadSystem.Load(SaveLoadSystem.SaveType.PatronBoard) as SavePatronDataV1;
        var floorB3 = FloorManager.Instance.GetFloor("B3");
        var patronBoard = floorB3.buildings[2] as PatronBoard;
        if (savePatronBoard != null)
        {
            string serverTimeString = UtilityTime.previousTimeData.EnterTime;
            DateTime now = DateTime.Parse(serverTimeString);

            if (savePatronBoard.dateTime.Day != now.Day)
            {
                patronBoard.isSaveFileLoaded = false;
            }
            else
            {
                hasPatronBoardDateTime = true;
                patronBoard.isSaveFileLoaded = true;

                for (int i = 0; i < savePatronBoard.patronboardSaveData.Count; ++i)
                {
                    if (savePatronBoard.patronboardSaveData[i].isCompleted)
                    {
                        continue;
                    }
                    patronBoard.requests.Add(savePatronBoard.patronboardSaveData[i].id);
                    patronBoard.exchangeStats.Add(new ExchangeStat(savePatronBoard.patronboardSaveData[i].id));
                }
            }
        }
        else
        {
            patronBoard.isSaveFileLoaded = false;
        }
        isLoadedWorld = true;


        await UniTask.WaitForSeconds(1);

    }

    public void RegisterSceneManager(SceneIds sceneName, SceneController sceneManager)
    {
        if (!sceneManagers.ContainsKey(sceneName))
        {
            sceneManagers[sceneName] = sceneManager;
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

    private void LoadSceneAsync(int sceneIndex)
    {
        CurrentSceneId = (SceneIds)sceneIndex;
        SceneManager.LoadSceneAsync(sceneIndex);
    }

    public AnimalManager GetAnimalManager()
    {
        if (CurrentSceneId == SceneIds.WorldLandOfHope)
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

        while (!DataTableMgr.GetItemTable().IsLoaded)
        {
            await UniTask.Yield();
        }

        while (!DataTableMgr.GetInvitationTable().IsLoaded)
        {
            await UniTask.Yield();
        }

        while (!DataTableMgr.GetMissionTable().IsLoaded)
        {
            await UniTask.Yield();
        }

        while (!DataTableMgr.GetSynergyTable().IsLoaded)
        {
            await UniTask.Yield();
        }

        while (!DataTableMgr.GetRewardTable().IsLoaded)
        {
            await UniTask.Yield();
        }

        while (!DataTableMgr.GetExchangeTable().IsLoaded)
        {
            await UniTask.Yield();
        }
        return;
    }

    public void SetPlayerData()
    {
        if (this == null)
        {
            return;
        }
        var saveData = new SaveDataV1();
        var saveCurrencyData = new SaveCurrencyDataV1();
        var saveProductData = new SaveProductDataV1();
        var saveCurrencyProductData = new SaveCurrencyProductDataV1();
        var savePatronboardData = new SavePatronDataV1();

        for (int i = 0; i < FloorManager.Instance.floors.Count; ++i)
        {
            if (i >= 5)
                break;

            string format = $"B{i + 1}";
            var floor = FloorManager.Instance.GetFloor(format);
            saveData.floors.Add(new FloorSaveData(floor.FloorStat));
            foreach (var animal in floor.animals)
            {
                saveData.floors[saveData.floors.Count - 1].animalSaveDatas.Add(new AnimalSaveData(animal.animalStat));
            }

            foreach (var building in floor.buildings)
            {
                saveData.floors[saveData.floors.Count - 1].buildingSaveDatas.Add(new BuildingSaveData(building.BuildingStat));
            }

            if (floor.storage != null)
                saveData.floors[saveData.floors.Count - 1].furnitureSaveDatas.Add(new FurnitureSaveData(floor.storage.BuildingStat)); // â��
        }

        SaveLoadSystem.Save(saveData);

        for (int i = 0; i < CurrencyManager.currencyTypes.Length; ++i)
        {
            saveCurrencyData.currencySaveData.Add(new CurrencySaveData(CurrencyManager.currencyTypes[i], CurrencyManager.currency[CurrencyManager.currencyTypes[i]]));
        }

        SaveLoadSystem.Save(saveCurrencyData, SaveLoadSystem.SaveType.Currency);
        var buildingFloor = FloorManager.Instance.GetFloor("B3") as CraftingFloor;
        for (int i = 0; i < buildingFloor.buildings.Count; ++i)
        {
            if (buildingFloor.buildings[i].BuildingStat.IsLock)
                continue;

            var building = buildingFloor.buildings[i];
            if ((building as CraftingBuilding) == null)
                continue;

            if ((building as CraftingBuilding).isCrafting)
            {
                foreach (var res in (building as CraftingBuilding).CurrentRecipeStat.Resources)
                {
                    CurrencyManager.product[(CurrencyProductType)res.Key] += res.Value;
                }

                foreach (var recipe in (building as CraftingBuilding).recipeStatList)
                {
                    foreach (var res in recipe.Resources)
                    {
                        CurrencyManager.product[(CurrencyProductType)res.Key] += res.Value;
                    }
                }
            }
        }

        for (int i = 0; i < CurrencyManager.productTypes.Length; ++i)
        {
            saveCurrencyProductData.currencySaveData.Add(new CurrencyProductSaveData(CurrencyManager.productTypes[i], CurrencyManager.product[CurrencyManager.productTypes[i]]));
        }

        SaveLoadSystem.Save(saveCurrencyProductData, SaveLoadSystem.SaveType.CurrencyProduct);

        var storageProduct = FloorManager.Instance.GetFloor("B3").storage as StorageProduct;
        for (int i = 0; i < storageProduct.Products.Count; ++i)
        {
            saveProductData.productSaveData.Add(new ProductSaveData(storageProduct.Products.ElementAt(i).Key, storageProduct.Products.ElementAt(i).Value));
        }
        SaveLoadSystem.Save(saveProductData, SaveLoadSystem.SaveType.Product);
        var patronboard = FloorManager.Instance.GetFloor("B3").buildings[2] as PatronBoard;
        if (!patronboard.BuildingStat.IsLock)
        {
            for (int i = 0; i < patronboard.requests.Count; ++i)
            {
                savePatronboardData.patronboardSaveData.Add(new PatronBoardSaveData(patronboard.requests[i], patronboard.exchangeStats[i].IsCompleted));
            }
            SaveLoadSystem.Save(savePatronboardData, SaveLoadSystem.SaveType.PatronBoard);

        }
    }

    public bool ShouldBeCreatedInScene(string sceneName)
    {
        return sceneName == "SampleScene CBTJH";
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        if (applicationIsQuitting)
            return;

        if (!ShouldBeCreatedInScene(SceneManager.GetActiveScene().name))
        {
            _instance = null;
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
        }
    }
}