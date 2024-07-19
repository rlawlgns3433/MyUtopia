using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class StorageConduct : Storage
{
    public List<CurrencyType> currencyTypes;
    public BigNumber CurrWorkLoad;
    private Building[] buildings;
    private bool isClick = false;
    private int currentTotalSeconds;
    public int CurrentTotalSeconds
    {
        get
        {
            return currentTotalSeconds;
        }
        private set
        {
            currentTotalSeconds = value;
        }
    }
    private BigNumber[] currArray;
    public BigNumber[] CurrArray
    {
        get
        {
            return currArray;
        }
        set
        {
            currArray = value;
        }
    }

    public Slider currentValue;

    public List<ParticleSystem> particleSystems;
    public Canvas canvas;
    private BigNumber[] values;
    public BigNumber[] Values
    {
        get
        {
            return values;
        }
        set
        {
            values = value;
        }
    }
    public Building[] Buildings
    {
        get
        {
            return buildings;
        }
        set
        {
            buildings = value;
        }
    }
    private int maxSeconds;
    public int MaxSeconds
    {
        get
        {
            return maxSeconds;
        }
        private set
        {
            maxSeconds = value;
        }
    }
    private int offLineSeconds;

    private StorageValue storageValue;
    private bool isAddQuitEvent = false;
    private void Awake()
    {
        clickEvent += OpenStorage;
        RegisterClickable();
        if (!isAddQuitEvent)
        {
            Application.quitting -= SaveDataOnQuit;
            Application.quitting += SaveDataOnQuit;
            isAddQuitEvent = true;
        }
    }

    private async void Start()
    {
        Debug.Log("Start method entered");
        try
        {
            await UniWaitFurnitureTable();
            Debug.Log("furnitureStat.Effect_Value > 0");
            MaxSeconds = FurnitureStat.Effect_Value;
            Debug.Log($"MaxSeconds set to {MaxSeconds}");
            Debug.Log($"{FurnitureStat.Effect_Type}/{FurnitureStat.Effect_Value}");
            await UniTask.WaitUntil(() => UtilityTime.Seconds > 0);
            Debug.Log("UtilityTime.Seconds > 0");

            buildings = new Building[currencyTypes.Count];
            Debug.Log("Buildings array initialized");

            CurrArray = new BigNumber[currencyTypes.Count];
            Debug.Log("CurrArray initialized");

            values = new BigNumber[currencyTypes.Count];
            Debug.Log("Values array initialized");

            LoadDataOnStart();
            Debug.Log("Data loaded on start");

            if (MaxSeconds == 0)
            {
                Debug.LogError("MaxSeconds is 0, which is invalid.");
                return;
            }

            Debug.Log($"UtilityTime.Seconds: {UtilityTime.Seconds}");
            currentTotalSeconds += UtilityTime.Seconds;

            if (maxSeconds > currentTotalSeconds)
            {
                offLineSeconds = UtilityTime.Seconds / 3;
            }
            else
            {
                var overSeconds = currentTotalSeconds - maxSeconds;
                var overTime = UtilityTime.Seconds - overSeconds;
                offLineSeconds = overTime / 3;
                if (offLineSeconds <= 0)
                {
                    offLineSeconds = 0;
                }
                currentTotalSeconds = maxSeconds;
            }
            Debug.Log($"offLineSeconds: {offLineSeconds}, UtilityTime.Seconds: {UtilityTime.Seconds}, currentTotalSeconds: {currentTotalSeconds}");

            await CheckStorage();
            Debug.Log("CheckStorage completed");

            if (currentTotalSeconds > 0)
            {
                if (currentValue == null)
                {
                    Debug.LogError("currentValue is null");
                    return;
                }
                currentValue.gameObject.SetActive(true);

                storageValue = currentValue.GetComponent<StorageValue>();
                if (storageValue == null)
                {
                    Debug.LogError("StorageValue component not found");
                    return;
                }
                currentValue.value = Mathf.Clamp01((float)currentTotalSeconds / maxSeconds);
                await UniTask.WaitUntil(() => currentValue.gameObject.activeSelf);
                storageValue.TotalValue = currentTotalSeconds;
                if (storageValue.TotalValue <= 0 || CurrWorkLoad == 0)
                {
                    currentValue.gameObject.SetActive(false);
                }
            }
            Debug.Log($"StorageChildTest: {storageValue?.TotalValue}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Exception in Start: {ex.Message}\n{ex.StackTrace}");
        }
    }

    public async UniTask CheckStorage()
    {
        await UniTask.WaitUntil(() => buildings.Length > 0 && buildings[0] != null);

        for (int i = 0; i < buildings.Length; i++)
        {
            var workRequire = buildings[i].BuildingStat.Work_Require;
            values[i] = CurrWorkLoad / workRequire;
            var tempValue = values[i] * offLineSeconds;
            CurrArray[i] += tempValue;
            Debug.Log($"Building {i}: workRequire = {workRequire}, values[i] = {values[i]}, tempValue = {tempValue}");
        }
        await UniTask.Yield();
    }

    private void SaveDataOnQuit()
    {
        Debug.Log("Application quitting, saving data...");
        SaveData();
    }

    private void LoadDataOnStart()
    {
        string filePath = Path.Combine(Application.persistentDataPath, $"{FurnitureStat.Furniture_ID}.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            StorageData data = JsonConvert.DeserializeObject<StorageData>(json, new WorkLoadConverter());
            if (data.CurrentWorkLoad.IsZero)
            {
                CurrWorkLoad = BigNumber.Zero;
            }
            else
            {
                CurrWorkLoad = data.CurrentWorkLoad;
            }
            if (data.CurrArray.Length != 0)
            {
                CurrArray = data.CurrArray;
            }
            if (data.TotalOfflineTime == 0)
            {
                currentTotalSeconds = 0;
            }
            else
            {
                currentTotalSeconds = data.TotalOfflineTime;
            }
            Debug.Log("Loaded data on start: " + json);
        }
        else
        {
            Debug.Log("No existing data file found.");
        }
    }

    public void OpenStorage()
    {
        currentValue.gameObject.SetActive(false);
        if (!isClick)
        {
            isClick = true;
            if (CurrArray != null)
            {
                for (int i = 0; i < CurrArray.Length; ++i)
                {
                    if (CurrArray[i] > BigNumber.Zero)
                    {
                        Debug.Log($"Emitting particle for currency index {i}, value {CurrArray[i]}");
                        ParticleSystemEmit(particleSystems[i]).Forget();
                    }
                }
            }
        }
    }

    public async UniTask ParticleSystemEmit(ParticleSystem ps)
    {
        if (ps != null)
        {
            var worldPosition = transform.position;
            var screenPos = Camera.main.WorldToScreenPoint(worldPosition);
            ps.transform.position = screenPos;

            ps.Emit(1);
            await UniTask.WaitUntil(() => !ps.IsAlive(true));
            if (isClick)
            {
                currentTotalSeconds = default;
            }
            Debug.Log("Click");
            for (int i = 0; i < currencyTypes.Count; ++i)
            {
                CurrencyManager.currency[currencyTypes[i]] += CurrArray[i];
                CurrArray[i] = BigNumber.Zero;

                isClick = true;
            }
        }
    }

    private void SaveData()
    {
        Debug.Log("Saving data...");
        if (CurrWorkLoad == 0)
        {
            CurrWorkLoad = BigNumber.Zero;
        }
        if (CurrArray == null)
        {
            CurrArray = new BigNumber[currencyTypes.Count];
        }
        if (UtilityTime.Seconds == 0 || CurrWorkLoad == 0)
        {
            currentTotalSeconds = 0;
        }
        string filePath = Path.Combine(Application.persistentDataPath, $"{FurnitureStat.Furniture_ID}.json");
        StorageData storageData = new StorageData
        {
            CurrentWorkLoad = CurrWorkLoad,
            CurrArray = CurrArray,
            TotalOfflineTime = currentTotalSeconds,
        };
        string json = JsonConvert.SerializeObject(storageData, Formatting.Indented, new WorkLoadConverter());
        Debug.Log("Serialized JSON: " + json);
        File.WriteAllText(filePath, json);
        Debug.Log("Data saved: " + json);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Debug.Log("Application paused, saving data...");
            SaveData();
        }
    }
    public async UniTask UniWaitFurnitureTable()
    {
        while (!DataTableMgr.GetFurnitureTable().IsLoaded)
        {
            await UniTask.Yield();
        }
        Debug.Log("FurnitureTable IsLoaded!!");
        return;
    }
}
